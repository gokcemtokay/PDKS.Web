// PDKS.WebUI/Controllers/CihazController.cs

using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Threading.Tasks;

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
    public class CihazController : ControllerBase
    {
        private readonly ICihazService _cihazService;

        public CihazController(ICihazService cihazService)
        {
            _cihazService = cihazService;
        }

        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }

        // GET: api/Cihaz
        [HttpGet]
        public async Task<IActionResult> GetCihazlar()
        {
            try
            {
                // ⭐ ŞİRKET BAZLI FİLTRELEME
                var sirketId = GetCurrentSirketId();
                var cihazlar = await _cihazService.GetBySirketAsync(sirketId); // ← DEĞİŞİKLİK
                return Ok(cihazlar);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Cihaz/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCihazById(int id)
        {
            try
            {
                var cihaz = await _cihazService.GetByIdAsync(id);
                if (cihaz == null)
                {
                    return NotFound($"Cihaz with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ
                var sirketId = GetCurrentSirketId();
                if (cihaz.SirketId != sirketId)
                {
                    return Forbid("Bu cihaz, yetkili olduğunuz şirkete ait değildir.");
                }

                return Ok(cihaz);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Cihaz
        [HttpPost]
        public async Task<IActionResult> CreateCihaz([FromBody] CihazCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ ŞİRKET ID'Sİ OTOMATIK ATAMA
                var sirketId = GetCurrentSirketId();
                dto.SirketId = sirketId; // ← DEĞİŞİKLİK

                var newId = await _cihazService.CreateAsync(dto);
                var createdCihaz = await _cihazService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetCihazById), new { id = newId }, createdCihaz);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Cihaz/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCihaz(int id, [FromBody] CihazUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Cihaz ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK KONTROLÜ
                var sirketId = GetCurrentSirketId();
                var mevcutCihaz = await _cihazService.GetByIdAsync(id);

                if (mevcutCihaz == null)
                {
                    return NotFound("Cihaz bulunamadı.");
                }

                if (mevcutCihaz.SirketId != sirketId)
                {
                    return Forbid("Bu cihazı güncelleme yetkiniz yok.");
                }

                dto.SirketId = sirketId; // ← ŞİRKET ID'Sİ KORUMA
                await _cihazService.UpdateAsync(dto);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
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

        // DELETE: api/Cihaz/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCihaz(int id)
        {
            try
            {
                // ⭐ GÜVENLİK KONTROLÜ
                var sirketId = GetCurrentSirketId();
                var cihaz = await _cihazService.GetByIdAsync(id);

                if (cihaz == null)
                {
                    return NotFound("Cihaz bulunamadı.");
                }

                if (cihaz.SirketId != sirketId)
                {
                    return Forbid("Bu cihazı silme yetkiniz yok.");
                }

                await _cihazService.DeleteAsync(id);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Cihaz/loglar/5
        [HttpGet("loglar/{cihazId}")]
        public async Task<IActionResult> GetCihazLoglari(int cihazId)
        {
            try
            {
                // ⭐ GÜVENLİK KONTROLÜ
                var sirketId = GetCurrentSirketId();
                var cihaz = await _cihazService.GetByIdAsync(cihazId);

                if (cihaz == null)
                {
                    return NotFound("Cihaz bulunamadı.");
                }

                if (cihaz.SirketId != sirketId)
                {
                    return Forbid("Bu cihazın loglarını görüntüleme yetkiniz yok.");
                }

                var loglar = await _cihazService.GetCihazLoglariAsync(cihazId);
                return Ok(loglar);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}