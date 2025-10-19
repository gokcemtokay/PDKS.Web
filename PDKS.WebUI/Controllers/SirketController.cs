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
    [Route("api/[controller]")]
    public class SirketController : ControllerBase
    {
        private readonly ISirketService _sirketService;

        public SirketController(ISirketService sirketService)
        {
            _sirketService = sirketService;
        }

        // GET: api/Sirket
        [HttpGet]
        public async Task<IActionResult> GetSirketler()
        {
            try
            {
                var sirketler = await _sirketService.GetAllSirketlerAsync();
                return Ok(sirketler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Sirket/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSirketById(int id)
        {
            try
            {
                var sirket = await _sirketService.GetSirketByIdAsync(id);
                if (sirket == null)
                {
                    return NotFound($"Şirket with ID {id} not found.");
                }
                return Ok(sirket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Sirket
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSirket([FromBody] SirketCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newSirketId = await _sirketService.CreateSirketAsync(dto);
                var createdSirket = await _sirketService.GetSirketByIdAsync(newSirketId);
                return CreatedAtAction(nameof(GetSirketById), new { id = newSirketId }, createdSirket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Sirket/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSirket(int id, [FromBody] SirketUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Sirket ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _sirketService.UpdateSirketAsync(dto);
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

        // POST: api/Sirket/transfer
        [HttpPost("transfer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransferPersonel([FromBody] PersonelTransferDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transferiYapanKullaniciId = GetCurrentUserId();
                var success = await _sirketService.TransferPersonelAsync(dto, transferiYapanKullaniciId);

                if (success)
                {
                    return Ok(new { message = "Personel başarıyla transfer edildi." });
                }
                else
                {
                    return BadRequest("Transfer işlemi başarısız oldu.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Sirket/transfer-history/123  (Personel ID'si)
        [HttpGet("transfer-history/{personelId}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> GetTransferHistory(int personelId)
        {
            try
            {
                var transferGecmisi = await _sirketService.GetPersonelTransferGecmisiAsync(personelId);
                return Ok(transferGecmisi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Helper metot: Token içerisinden o anki kullanıcının ID'sini okur.
        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            // Bu durum normalde [Authorize] olduğu için yaşanmamalı.
            throw new InvalidOperationException("User ID could not be found in token.");
        }
    }
}