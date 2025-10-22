using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
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
    public class GirisCikisController : ControllerBase
    {
        private readonly IGirisCikisService _girisCikisService;

        public GirisCikisController(IGirisCikisService girisCikisService)
        {
            _girisCikisService = girisCikisService;
        }

        // GET: api/GirisCikis?tarih=2023-10-27
        [HttpGet]
        public async Task<IActionResult> GetGirisCikislar([FromQuery] DateTime? tarih)
        {
            try
            {
                // Eğer tarih belirtilmemişse bugünün tarihini kullan
                var selectedDate = tarih ?? DateTime.Today;
                var girisCikislar = await _girisCikisService.GetByDateAsync(selectedDate);
                return Ok(girisCikislar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/GirisCikis/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGirisCikisById(int id)
        {
            try
            {
                var girisCikis = await _girisCikisService.GetByIdAsync(id);
                if (girisCikis == null)
                {
                    return NotFound($"Kayıt with ID {id} not found.");
                }
                return Ok(girisCikis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/GirisCikis
        [HttpPost]
        public async Task<IActionResult> CreateGirisCikis([FromBody] GirisCikisCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newId = await _girisCikisService.CreateAsync(dto);
                var createdRecord = await _girisCikisService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetGirisCikisById), new { id = newId }, createdRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/GirisCikis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGirisCikis(int id, [FromBody] GirisCikisUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Record ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _girisCikisService.UpdateAsync(dto);
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

        // DELETE: api/GirisCikis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGirisCikis(int id)
        {
            try
            {
                await _girisCikisService.DeleteAsync(id);
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
    }
}