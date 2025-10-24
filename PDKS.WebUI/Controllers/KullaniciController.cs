using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciController(IKullaniciService kullaniciService, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _kullaniciService = kullaniciService;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Kullanici
        [HttpGet]
        public async Task<IActionResult> GetKullanicilar()
        {
            try
            {
                var kullanicilar = await _kullaniciService.GetAllAsync();
                return Ok(kullanicilar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Kullanici/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKullaniciById(int id)
        {
            try
            {
                var kullanici = await _kullaniciService.GetByIdAsync(id);
                if (kullanici == null)
                {
                    return NotFound($"Kullanıcı with ID {id} not found.");
                }
                return Ok(kullanici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Kullanici
        [HttpPost]
        public async Task<IActionResult> CreateKullanici([FromBody] KullaniciCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Şifreyi hash'leyerek DTO'yu servise gönder
                dto.Sifre = _authService.HashPassword(dto.Sifre);
                var newId = await _kullaniciService.CreateAsync(dto);
                var createdUser = await _kullaniciService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetKullaniciById), new { id = newId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Kullanici/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKullanici(int id, [FromBody] KullaniciUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Kullanıcı ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Eğer yeni bir şifre gönderildiyse hash'le, değilse mevcut şifreyi koru.
                if (!string.IsNullOrEmpty(dto.YeniSifre))
                {
                    dto.Sifre = _authService.HashPassword(dto.YeniSifre);
                }

                await _kullaniciService.UpdateAsync(dto);
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

        [HttpGet("yetkili-sirketler")]
        [Authorize]
        public async Task<IActionResult> GetYetkiliSirketler()
        {
            try
            {
                var kullaniciId = GetCurrentUserId();

                var yetkiliSirketler = await _unitOfWork.KullaniciSirketler
                    .FindAsync(ks => ks.KullaniciId == kullaniciId && ks.Aktif);

                var sirketler = new List<object>();

                foreach (var ks in yetkiliSirketler)
                {
                    var sirket = await _unitOfWork.Sirketler.GetByIdAsync(ks.SirketId);
                    if (sirket != null)
                    {
                        sirketler.Add(new
                        {
                            id = sirket.Id,
                            unvan = sirket.Unvan,
                            logoUrl = sirket.LogoUrl,
                            varsayilan = ks.Varsayilan
                        });
                    }
                }

                return Ok(sirketler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
            }
        }

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
    }
}