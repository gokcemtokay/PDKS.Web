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
    public class ParametreController : ControllerBase
    {
        private readonly IParametreService _parametreService;

        public ParametreController(IParametreService parametreService)
        {
            _parametreService = parametreService;
        }

        // GET: api/Parametre
        [HttpGet]
        public async Task<IActionResult> GetParametreler()
        {
            try
            {
                var parametreler = await _parametreService.GetAllAsync();
                return Ok(parametreler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Parametre/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParametreById(int id)
        {
            try
            {
                var parametre = await _parametreService.GetByIdAsync(id);
                if (parametre == null)
                {
                    return NotFound($"Parametre with ID {id} not found.");
                }
                return Ok(parametre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Parametre/kategori/GOREV
        [HttpGet("kategori/{kategori}")]
        public async Task<IActionResult> GetByKategori(string kategori)
        {
            try
            {
                var parametreler = await _parametreService.GetByKategoriAsync(kategori);
                return Ok(parametreler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Parametre
        [HttpPost]
        public async Task<IActionResult> CreateParametre([FromBody] ParametreCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newId = await _parametreService.CreateAsync(dto);
                var createdParametre = await _parametreService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetParametreById), new { id = newId }, createdParametre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Parametre/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParametre(int id, [FromBody] ParametreUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Parametre ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _parametreService.UpdateAsync(dto);
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

        // DELETE: api/Parametre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParametre(int id)
        {
            try
            {
                await _parametreService.DeleteAsync(id);
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