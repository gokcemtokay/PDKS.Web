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
    public class TatilController : ControllerBase
    {
        private readonly ITatilService _tatilService;

        public TatilController(ITatilService tatilService)
        {
            _tatilService = tatilService;
        }

        // GET: api/Tatil
        [HttpGet]
        public async Task<IActionResult> GetTatiller()
        {
            try
            {
                var tatiller = await _tatilService.GetAllAsync();
                return Ok(tatiller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Tatil
        [HttpPost]
        public async Task<IActionResult> CreateTatil([FromBody] TatilCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newId = await _tatilService.CreateAsync(dto);
                // Başarılı oluşturma sonrası 201 Created durum kodu ve oluşturulan kaynağın bilgisi döndürülebilir.
                // Şimdilik basitçe OK dönelim.
                return Ok(new { id = newId });
            }
            catch (Exception ex)
            {
                // Tarih çakışması gibi özel hataları yakalayıp 409 Conflict döndürmek daha iyi bir pratiktir.
                if (ex.Message.Contains("zaten mevcut"))
                {
                    return Conflict(ex.Message);
                }
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // DELETE: api/Tatil/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTatil(int id)
        {
            try
            {
                await _tatilService.DeleteAsync(id);
                return NoContent(); // Başarılı silme işleminde 204 No Content döndürülür.
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