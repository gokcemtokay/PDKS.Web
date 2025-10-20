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
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CihazController : ControllerBase
    {
        private readonly ICihazService _cihazService;

        public CihazController(ICihazService cihazService)
        {
            _cihazService = cihazService;
        }

        // GET: api/Cihaz
        [HttpGet]
        public async Task<IActionResult> GetCihazlar()
        {
            try
            {
                var cihazlar = await _cihazService.GetAllAsync();
                return Ok(cihazlar);
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
                return Ok(cihaz);
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
                var newId = await _cihazService.CreateAsync(dto);
                var createdCihaz = await _cihazService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetCihazById), new { id = newId }, createdCihaz);
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
                await _cihazService.UpdateAsync(dto);
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

        // GET: api/Cihaz/loglar/5
        [HttpGet("loglar/{cihazId}")]
        public async Task<IActionResult> GetCihazLoglari(int cihazId)
        {
            try
            {
                var loglar = await _cihazService.GetCihazLoglariAsync(cihazId);
                return Ok(loglar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}