using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SirketController : ControllerBase
    {
        private readonly ISirketService _sirketService;
        private readonly IConfiguration _configuration; 
        private readonly IUnitOfWork _unitOfWork;

        public SirketController(ISirketService sirketService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _sirketService = sirketService;
            _configuration = configuration; 
            _unitOfWork = unitOfWork; 
        }

        // GET: api/Sirket
        [HttpGet]
        public async Task<IActionResult> GetSirketler()
        {
            try
            {
                var sirketler = await _sirketService.GetAllSirketlerAsync();
                return Ok(sirketler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Sirket/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSirketById(int id)
        {
            try
            {
                var sirket = await _sirketService.GetSirketByIdAsync(id);
                if (sirket == null)
                {
                    return NotFound($"Şirket with ID {id} not found.");
                }
                return Ok(sirket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Sirket
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSirket([FromBody] SirketCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newSirketId = await _sirketService.CreateSirketAsync(dto);
                var createdSirket = await _sirketService.GetSirketByIdAsync(newSirketId);
                return CreatedAtAction(nameof(GetSirketById), new { id = newSirketId }, createdSirket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Sirket/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSirket(int id, [FromBody] SirketUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Sirket ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _sirketService.UpdateSirketAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Sirket/transfer
        [HttpPost("transfer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransferPersonel([FromBody] PersonelTransferDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transferiYapanKullaniciId = GetCurrentUserId();
                var success = await _sirketService.TransferPersonelAsync(dto, transferiYapanKullaniciId);

                if (success)
                {
                    return Ok(new { message = "Personel başarıyla transfer edildi." });
                }
                else
                {
                    return BadRequest("Transfer işlemi başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Sirket/transfer-history/123  (Personel ID'si)
        [HttpGet("transfer-history/{personelId}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> GetTransferHistory(int personelId)
        {
            try
            {
                var transferGecmisi = await _sirketService.GetPersonelTransferGecmisiAsync(personelId);
                return Ok(transferGecmisi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Helper metodlar
        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier
                                                              || c.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID could not be found in token.");
        }


        [HttpPost("switch/{sirketId}")]
        [Authorize]
        public async Task<IActionResult> SwitchSirket(int sirketId)
        {
            try
            {
                var kullaniciId = GetCurrentUserId();

                // Kullanıcının bu şirkete yetkisi var mı kontrol et
                var yetki = await _unitOfWork.KullaniciSirketler
                    .FirstOrDefaultAsync(ks =>
                        ks.KullaniciId == kullaniciId &&
                        ks.SirketId == sirketId &&
                        ks.Aktif);

                if (yetki == null)
                {
                    return Forbid("Bu şirkete erişim yetkiniz yok.");
                }

                // Kullanıcı ve şirket bilgisini al
                var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
                if (kullanici == null)
                {
                    return NotFound(new { message = "Kullanıcı bulunamadı." });
                }

                kullanici.Rol = await _unitOfWork.Roller.GetByIdAsync(kullanici.RolId);
                var sirket = await _unitOfWork.Sirketler.GetByIdAsync(sirketId);

                // Yeni token oluştur
                var newToken = GenerateJwtToken(kullanici, sirketId, sirket.Unvan);

                return Ok(new
                {
                    token = newToken,
                    sirket = new
                    {
                        id = sirket.Id,
                        unvan = sirket.Unvan,
                        logoUrl = sirket.LogoUrl
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
            }
        }

        private string GenerateJwtToken(Kullanici kullanici, int sirketId, string sirketAdi)
        {
            // Yukarıdaki GenerateJwtToken metodu
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, kullanici.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, kullanici.Email),
                new Claim(ClaimTypes.Role, kullanici.Rol.RolAdi),
                new Claim("personelId", kullanici.PersonelId.ToString()),
                new Claim("sirketId", sirketId.ToString()),
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

        // Token'dan şirket ID'sini al
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new InvalidOperationException("Şirket ID could not be found in token.");
        }
    }
}