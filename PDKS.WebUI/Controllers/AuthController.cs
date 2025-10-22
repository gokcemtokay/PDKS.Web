using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using PDKS.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciService _kullaniciService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IConfiguration configuration, IKullaniciService kullaniciService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _configuration = configuration;
            _kullaniciService = kullaniciService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Kullanıcıyı doğrula (Şifre kontrolü)
            var kullanici = await _authService.ValidateUserAsync(loginDto.Email, loginDto.Password);

            if (kullanici == null)
            {
                return Unauthorized(new { message = "Geçersiz e-posta veya şifre." });
            }

            // 2. CRITICAL FIX: Kullanıcının Rol'ünü çek (NRE hatasını gidermek için)
            var rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);

            if (rol == null)
            {
                return StatusCode(500, new { message = "Kullanıcının rol bilgisi veritabanında eksik." });
            }
            kullanici.Rol = rol;

            // 3. Kullanıcının yetkili olduğu KullaniciSirket kayıtlarını getir
            var yetkiliSirketKayitlari = await _unitOfWork.GetRepository<KullaniciSirket>()
                .FindAsync(ks => ks.KullaniciId == kullanici.Id && ks.Aktif);

            if (!yetkiliSirketKayitlari.Any())
            {
                return Unauthorized(new { message = "Kullanıcıya ait aktif şirket bulunamadı. Lütfen sistem yöneticinize başvurun." });
            }

            // 4. Varsayılan şirketi bul veya ilk şirketi kullan
            // NOT: Varsayilan alanı KullaniciSirket entity'sinde bulunmalıdır.
            var varsayilanKayit = yetkiliSirketKayitlari.FirstOrDefault(ks => ks.Varsayilan);
            var aktifKayit = varsayilanKayit ?? yetkiliSirketKayitlari.First();

            // 5. Aktif şirketin detaylarını çek (token ve ana ekran için)
            var aktifSirket = await _unitOfWork.Sirketler.GetByIdAsync(aktifKayit.SirketId);

            if (aktifSirket == null)
            {
                return StatusCode(500, new { message = "Seçilen şirket bilgisi veritabanında bulunamadı." });
            }

            // 6. Yetkili şirketler listesi için tüm detayları çek ve DTO listesi oluştur (NRE'siz Projeksiyon)
            var yetkiliSirketDetaylari = new List<object>();

            // Her bir yetkili şirketin detayını çekiyoruz (Eager Loading desteği olmadığı varsayımıyla NRE'yi önlüyoruz)
            foreach (var kayit in yetkiliSirketKayitlari)
            {
                var sirketDetay = await _unitOfWork.Sirketler.GetByIdAsync(kayit.SirketId);

                if (sirketDetay != null)
                {
                    yetkiliSirketDetaylari.Add(new
                    {
                        id = sirketDetay.Id,
                        unvan = sirketDetay.Unvan,
                        logoUrl = sirketDetay.LogoUrl,
                        varsayilan = kayit.Varsayilan
                    });
                }
            }


            // 7. JWT Token oluştur
            var token = GenerateJwtToken(kullanici, aktifSirket.Id, aktifSirket.Unvan);

            return Ok(new
            {
                token = token,
                kullanici = new
                {
                    id = kullanici.Id,
                    email = kullanici.Email,
                    rol = kullanici.Rol.RolAdi,
                    personelId = kullanici.PersonelId
                },
                aktifSirket = new
                {
                    id = aktifSirket.Id,
                    unvan = aktifSirket.Unvan,
                    logoUrl = aktifSirket.LogoUrl
                },
                yetkiliSirketler = yetkiliSirketDetaylari
            });
        }

        [HttpPost("logout")]
        [Authorize] // Sadece token'ı olan bir kullanıcı bu isteği yapabilir.
        public IActionResult Logout()
        {
            // JWT stateless olduğu için sunucu tarafında yapılacak bir işlem yoktur.
            return Ok(new { message = "Çıkış başarılı. Lütfen token'ı client tarafında silin." });
        }


        private string GenerateJwtToken(Kullanici kullanici, int aktifSirketId, string sirketAdi)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol.RolAdi),
                new Claim("personelId", kullanici.PersonelId?.ToString() ?? ""),
                new Claim("sirketId", aktifSirketId.ToString()),
                new Claim("sirketAdi", sirketAdi),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("hash-password")]
        [AllowAnonymous]
        public IActionResult HashPassword([FromBody] HashPasswordRequest request)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            return Ok(new
            {
                password = request.Password,
                hash = hash,
                algorithm = "BCrypt"
            });
        }


        [HttpPost("create-bcrypt-hash")]
        [AllowAnonymous]
        public IActionResult CreateBCryptHash()
        {
            var password = "admin123";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            return Ok(new
            {
                password = password,
                hash = hash,
                hashLength = hash.Length,
                startsWithDollar = hash.StartsWith("$2"),
                testVerify = BCrypt.Net.BCrypt.Verify(password, hash)
            });
        }


        // DTO
        public class HashPasswordRequest
        {
            public string Password { get; set; } = string.Empty;
        }
    }
}