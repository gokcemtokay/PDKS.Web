// PDKS.WebUI/Controllers/KullaniciController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")]
#else
    [Route("[controller]")]
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IAuthService _authService;

        public KullaniciController(IKullaniciService kullaniciService, IAuthService authService)
        {
            _kullaniciService = kullaniciService;
            _authService = authService;
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
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
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
                    return NotFound(new { message = $"Kullanıcı bulunamadı (ID: {id})" });
                }

                return Ok(kullanici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
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
                // Şifreyi hash'le
                dto.Sifre = _authService.HashPassword(dto.Sifre);

                var newId = await _kullaniciService.CreateAsync(dto);
                var createdUser = await _kullaniciService.GetByIdAsync(newId);

                return CreatedAtAction(nameof(GetKullaniciById), new { id = newId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
            }
        }

        // PUT: api/Kullanici/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKullanici(int id, [FromBody] KullaniciUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID uyuşmazlığı" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Yeni şifre varsa hash'le
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
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
            }
        }

        // DELETE: api/Kullanici/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKullanici(int id)
        {
            try
            {
                await _kullaniciService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, new { message = $"Hata: {ex.Message}" });
            }
        }
    }
}
