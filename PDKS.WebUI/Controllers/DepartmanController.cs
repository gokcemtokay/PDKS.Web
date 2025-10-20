using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DepartmanController : ControllerBase
    {
        private readonly IDepartmanService _departmanService;

        public DepartmanController(IDepartmanService departmanService)
        {
            _departmanService = departmanService;
        }

        // GET: api/Departman
        [HttpGet]
        public async Task<IActionResult> GetDepartmanlar()
        {
            try
            {
                var departmanlar = await _departmanService.GetAllAsync();
                return Ok(departmanlar);
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
                return Ok(departman);
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
                var newDepartmanId = await _departmanService.CreateAsync(dto);
                var createdDepartman = await _departmanService.GetByIdAsync(newDepartmanId);
                return CreatedAtAction(nameof(GetDepartmanById), new { id = newDepartmanId }, createdDepartman);
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
                await _departmanService.UpdateAsync(dto);
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

        // DELETE: api/Departman/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartman(int id)
        {
            try
            {
                await _departmanService.DeleteAsync(id);
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