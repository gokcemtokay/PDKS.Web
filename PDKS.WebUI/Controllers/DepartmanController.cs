using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Linq;
using System.Security.Claims;
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
    public class DepartmanController : ControllerBase
    {
        private readonly IDepartmanService _departmanService;

        public DepartmanController(IDepartmanService departmanService)
        {
            _departmanService = departmanService;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // GET: api/Departman
        [HttpGet]
        public async Task<IActionResult> GetDepartmanlar()
        {
            try
            {
                // ⭐ GÜNCELLEME: JWT Token'dan şirket ID'sini al ve filtrele
                var sirketId = GetCurrentSirketId();
                // Bu çağrının Servis katmanında Departman.SirketId == sirketId filtresi yapması beklenir.
                var departmanlar = await _departmanService.GetBySirketAsync(sirketId);

                return Ok(departmanlar);
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

        // GET: api/Departman/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmanById(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);
                if (departman == null)
                {
                    return NotFound($"Departman with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Departmanın aktif şirkete ait olup olmadığını kontrol et
                // Not: Servis'ten dönen DTO'nun SirketId içermesi gerekir.
                var sirketId = GetCurrentSirketId();
                // DTO'da SirketId alanı olduğu varsayılmıştır.
                if (departman.SirketId != sirketId)
                {
                    return Forbid("Bu departman, yetkili olduğunuz şirkete ait değildir.");
                }

                return Ok(departman);
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

        // POST: api/Departman
        [HttpPost]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> CreateDepartman([FromBody] DepartmanCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK KONTROLÜ: DTO'daki şirket ID'si token'daki ID ile eşleşmeli
                var sirketId = GetCurrentSirketId();
                if (dto.SirketId != sirketId)
                {
                    return Forbid("Yeni departman kaydı, sadece aktif şirketinize yapılabilir.");
                }

                var newDepartmanId = await _departmanService.CreateAsync(dto);
                var createdDepartman = await _departmanService.GetByIdAsync(newDepartmanId);
                return CreatedAtAction(nameof(GetDepartmanById), new { id = newDepartmanId }, createdDepartman);
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

        // PUT: api/Departman/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> UpdateDepartman(int id, [FromBody] DepartmanUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Departman ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK KONTROLÜ: DTO'daki şirket ID'si token'daki ID ile eşleşmeli
                var sirketId = GetCurrentSirketId();
                if (dto.SirketId != sirketId)
                {
                    return Forbid("Departman güncellemesi, sadece aktif şirketinize yapılabilir.");
                }

                await _departmanService.UpdateAsync(dto);
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

        // DELETE: api/Departman/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartman(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);
                if (departman == null)
                {
                    return NotFound($"Departman with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Departmanın aktif şirkete ait olup olmadığını kontrol et
                var sirketId = GetCurrentSirketId();
                // DTO'da SirketId alanı olduğu varsayılmıştır.
                if (departman.SirketId != sirketId)
                {
                    return Forbid("Bu departman, yetkili olduğunuz şirkete ait değildir ve silinemez.");
                }

                await _departmanService.DeleteAsync(id);
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
    }
}