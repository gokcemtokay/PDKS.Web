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
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class VardiyaController : ControllerBase
    {
        private readonly IVardiyaService _vardiyaService;

        public VardiyaController(IVardiyaService vardiyaService)
        {
            _vardiyaService = vardiyaService;
        }

        // GET: api/Vardiya
        [HttpGet]
        public async Task<IActionResult> GetVardiyalar()
        {
            try
            {
                var vardiyalar = await _vardiyaService.GetAllAsync();
                return Ok(vardiyalar);
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
                return Ok(vardiya);
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
                var newId = await _vardiyaService.CreateAsync(dto);
                var createdVardiya = await _vardiyaService.GetByIdAsync(newId);
                return CreatedAtAction(nameof(GetVardiyaById), new { id = newId }, createdVardiya);
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
                await _vardiyaService.UpdateAsync(dto);
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