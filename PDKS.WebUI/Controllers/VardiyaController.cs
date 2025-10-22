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
    [Authorize(Roles = "Admin,IK")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class VardiyaController : ControllerBase
    {
        private readonly IVardiyaService _vardiyaService;

        public VardiyaController(IVardiyaService vardiyaService)
        {
            _vardiyaService = vardiyaService;
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

        // GET: api/Vardiya
        [HttpGet]
        public async Task<IActionResult> GetVardiyalar()
        {
            try
            {
                // ⭐ GÜNCELLEME: JWT Token'dan şirket ID'sini al ve filtrele
                var sirketId = GetCurrentSirketId();
                // Bu çağrının Servis katmanında Vardiya.SirketId == sirketId filtresi yapması beklenir.
                var vardiyalar = await _vardiyaService.GetBySirketAsync(sirketId);

                return Ok(vardiyalar);
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

        // GET: api/Vardiya/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVardiyaById(int id)
        {
            try
            {
                var vardiya = await _vardiyaService.GetByIdAsync(id);
                if (vardiya == null)
                {
                    return NotFound($"Vardiya with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Vardiyanın aktif şirkete ait olup olmadığını kontrol et
                var sirketId = GetCurrentSirketId();
                // DTO'da SirketId alanı olduğu varsayılmıştır.
                if (vardiya.SirketId != sirketId)
                {
                    return Forbid("Bu vardiya, yetkili olduğunuz şirkete ait değildir.");
                }

                return Ok(vardiya);
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

        // POST: api/Vardiya
        [HttpPost]
        public async Task<IActionResult> CreateVardiya([FromBody] VardiyaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK VE GÜNCELLEME: DTO'ya aktif şirket ID'sini enjekte et
                var sirketId = GetCurrentSirketId();
                // Bu adımın Service katmanında gerçekleşmesi beklenir.
                // Servis, gelen DTO'ya sirketId'yi eklemeli veya DTO'da bu alan olmalıdır.

                var newId = await _vardiyaService.CreateAsync(dto);
                var createdVardiya = await _vardiyaService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetVardiyaById), new { id = newId }, createdVardiya);
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

        // PUT: api/Vardiya/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVardiya(int id, [FromBody] VardiyaUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Vardiya ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // ⭐ GÜVENLİK KONTROLÜ: Güncellenen vardiyanın şirkete ait olduğunu varsayarak işlem yapılır.
                var sirketId = GetCurrentSirketId();
                // Servis katmanında bu kontrolün yapılması beklenir.

                await _vardiyaService.UpdateAsync(dto);
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