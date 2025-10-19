using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
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
                return Ok(parametreler); // DÜZELTİLEN SATIR
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
    }
}