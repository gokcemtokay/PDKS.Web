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
using DocumentFormat.OpenXml.Math;

namespace PDKS.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciService _kullaniciService;

        public AuthController(IAuthService authService, IConfiguration configuration, IKullaniciService kullaniciService)
        {
            _authService = authService;
            _configuration = configuration;
            _kullaniciService = kullaniciService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kullanici = await _authService.ValidateUserAsync(loginDto.Email, loginDto.Password);

            if (kullanici != null)
            {
                var token = GenerateJwtToken(kullanici);
                return Ok(new { token = token });
            }

            return Unauthorized("Geçersiz e-posta veya şifre.");
        }

        [HttpPost("logout")]
        [Authorize] // Sadece token'ı olan bir kullanıcı bu isteği yapabilir.
        public IActionResult Logout()
        {
            // JWT stateless olduğu için sunucu tarafında yapılacak bir işlem yoktur.
            // Token'ın silinmesi ve oturumun sonlandırılması client (React) tarafının sorumluluğundadır.
            // Bu endpoint, client'a işlemin başarılı olduğunu bildirmek için 200 OK döner.
            return Ok(new { message = "Çıkış başarılı. Lütfen token'ı client tarafında silin." });
        }


        private string GenerateJwtToken(Kullanici kullanici)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol.RolAdi),
                new Claim("personelId", kullanici.PersonelId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8), // Token geçerlilik süresi
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