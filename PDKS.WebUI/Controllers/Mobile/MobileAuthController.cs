using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Business.Services;
using PDKS.Data.Context;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System.Security.Claims;

namespace PDKS.WebUI.Controllers.Mobile
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/mobile/[controller]")]
    [ApiController]
    public class MobileAuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciService _kullaniciService;
        private readonly PDKSDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public MobileAuthController(
            IAuthService authService,
            IConfiguration configuration,
            IKullaniciService kullaniciService,
            PDKSDbContext context,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _configuration = configuration;
            _kullaniciService = kullaniciService;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // POST: api/v1/mobile/MobileAuth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MobileLoginDto dto)
        {
            var kullanici = await _authService.ValidateUserAsync(dto.Email, dto.Password);

            if (kullanici == null)
                return Unauthorized(new MobileResponse<object>
                {
                    Success = false,
                    Message = "Email veya şifre hatalı",
                    Data = null
                });

            // Rol bilgisi
            var rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);
            if (rol == null)
                return StatusCode(500, new MobileResponse<object>
                {
                    Success = false,
                    Message = "Rol bilgisi bulunamadı",
                    Data = null
                });

            kullanici.Rol = rol;

            // Personel bilgisi
            var personel = await _context.Personeller
                .Include(p => p.Departman)
                .FirstOrDefaultAsync(p => p.Id == kullanici.PersonelId);

            // Token oluştur
            var token = GenerateJwtToken(kullanici);

            // Son giriş tarihini güncelle
            kullanici.SonGirisTarihi = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // FCM Token kaydet (Push notification için)
            if (!string.IsNullOrEmpty(dto.FcmToken))
            {
                await SaveFcmToken(kullanici.Id, dto.FcmToken, dto.DeviceInfo);
            }

            return Ok(new MobileResponse<object>
            {
                Success = true,
                Message = "Giriş başarılı",
                Data = new
                {
                    Token = token,
                    User = new
                    {
                        kullanici.Id,
                        kullanici.Email,
                        PersonelId = kullanici.PersonelId,
                        AdSoyad = personel?.AdSoyad,
                        Rol = rol.RolAdi,
                        Departman = personel?.Departman?.Ad, // ✅ DepartmanAdi yerine Ad
                        ProfilResmi = personel?.ProfilResmi ?? (string?)null // ✅ Fotograf yerine ProfilResmi
                    }
                }
            });
        }

        // POST: api/v1/mobile/MobileAuth/refresh-token
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var kullanici = await _context.Kullanicilar
                .Include(k => k.Rol)
                .FirstOrDefaultAsync(k => k.Id == kullaniciId);

            if (kullanici == null)
                return Unauthorized(new MobileResponse<object>
                {
                    Success = false,
                    Message = "Geçersiz kullanıcı",
                    Data = null
                });

            var token = GenerateJwtToken(kullanici);

            return Ok(new MobileResponse<object>
            {
                Success = true,
                Message = "Token yenilendi",
                Data = new { Token = token }
            });
        }

        // POST: api/v1/mobile/MobileAuth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // FCM Token'ı sil
            if (!string.IsNullOrEmpty(dto.FcmToken))
            {
                var deviceToken = await _unitOfWork.GetRepository<DeviceToken>()
                    .FirstOrDefaultAsync(dt => dt.KullaniciId == kullaniciId && dt.Token == dto.FcmToken);

                if (deviceToken != null)
                {
                    _unitOfWork.GetRepository<DeviceToken>().Remove(deviceToken);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return Ok(new MobileResponse<object>
            {
                Success = true,
                Message = "Çıkış başarılı",
                Data = null
            });
        }

        // Helper Methods
        private string GenerateJwtToken(Kullanici kullanici)
        {
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol.RolAdi),
                new Claim("personelId", kullanici.PersonelId?.ToString() ?? ""),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30), // Mobil için daha uzun
                signingCredentials: credentials);

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task SaveFcmToken(int kullaniciId, string fcmToken, string? deviceInfo)
        {
            var existingToken = await _unitOfWork.GetRepository<DeviceToken>()
                .FirstOrDefaultAsync(dt => dt.KullaniciId == kullaniciId && dt.Token == fcmToken);

            if (existingToken == null)
            {
                var deviceToken = new DeviceToken
                {
                    KullaniciId = kullaniciId,
                    Token = fcmToken,
                    DeviceInfo = deviceInfo,
                    Platform = deviceInfo?.Contains("iOS") == true ? "iOS" : "Android",
                    OlusturmaTarihi = DateTime.UtcNow,
                    SonKullanimTarihi = DateTime.UtcNow,
                    Aktif = true
                };

                await _unitOfWork.GetRepository<DeviceToken>().AddAsync(deviceToken);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                existingToken.SonKullanimTarihi = DateTime.UtcNow;
                existingToken.Aktif = true;
                _unitOfWork.GetRepository<DeviceToken>().Update(existingToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    // DTOs
    public class MobileLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FcmToken { get; set; }
        public string? DeviceInfo { get; set; }
    }

    public class LogoutDto
    {
        public string? FcmToken { get; set; }
    }

    public class MobileResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public int? ErrorCode { get; set; }
    }
}