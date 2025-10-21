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
    [Authorize] // Bu controller'a erişim için yetkilendirme (geçerli token) gerekir.
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PersonelController : ControllerBase
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmanService _departmanService;
        private readonly IVardiyaService _vardiyaService;
        private readonly ISirketService _sirketService;

        public PersonelController(
            IPersonelService personelService,
            IDepartmanService departmanService,
            IVardiyaService vardiyaService,
            ISirketService sirketService)
        {
            _personelService = personelService;
            _departmanService = departmanService;
            _vardiyaService = vardiyaService;
            _sirketService = sirketService;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            // Token geçerli olduğu varsayılır, ancak sirketId yoksa hata fırlatılır.
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // GET: api/Personel
        [HttpGet]
        public async Task<IActionResult> GetPersoneller()
        {
            try
            {
                // ⭐ GÜNCELLEME: JWT Token'dan şirket ID'sini al ve filtrele
                var sirketId = GetCurrentSirketId();
                // Bu çağrının Servis katmanında Personel.SirketId == sirketId filtresi yapması beklenir.
                var personeller = await _personelService.GetBySirketAsync(sirketId);

                return Ok(personeller);
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

        // GET: api/Personel/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonelById(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    return NotFound($"Personel with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Personelin aktif şirkete ait olup olmadığını kontrol et
                // Not: Servis'ten dönen DTO'nun SirketId içermesi gerekir.
                // Assuming PersonelDetailDTO/PersonelListDTO has SirketId property.
                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                {
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir.");
                }

                return Ok(personel);
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

        // POST: api/Personel
        [HttpPost]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> CreatePersonel([FromBody] PersonelCreateDTO dto)
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
                    return Forbid("Yeni personel kaydı, sadece aktif şirketinize yapılabilir.");
                }

                var newPersonelId = await _personelService.CreateAsync(dto);
                var createdPersonel = await _personelService.GetByIdAsync(newPersonelId);
                return CreatedAtAction(nameof(GetPersonelById), new { id = newPersonelId }, createdPersonel);
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

        // PUT: api/Personel/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> UpdatePersonel(int id, [FromBody] PersonelUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Personel ID mismatch.");
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
                    return Forbid("Personel güncellemesi, sadece aktif şirketinize yapılabilir.");
                }

                await _personelService.UpdateAsync(dto);
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

        // DELETE: api/Personel/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePersonel(int id)
        {
            try
            {
                // Personelin SirketId'sini öğrenmek için PersonelService'ten çekilir
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    return NotFound($"Personel with ID {id} not found.");
                }

                // ⭐ GÜVENLİK KONTROLÜ: Personelin aktif şirkete ait olup olmadığını kontrol et
                var sirketId = GetCurrentSirketId();
                if (personel.SirketId != sirketId)
                {
                    return Forbid("Bu personel, yetkili olduğunuz şirkete ait değildir ve silinemez.");
                }

                await _personelService.DeleteAsync(id);
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