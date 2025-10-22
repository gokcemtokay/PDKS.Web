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
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class IzinController : ControllerBase
    {
        private readonly IIzinService _izinService;

        public IzinController(IIzinService izinService)
        {
            _izinService = izinService;
        }

        // GET: api/Izin
        [HttpGet]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> GetIzinler()
        {
            try
            {
                var izinler = await _izinService.GetAllAsync();
                return Ok(izinler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Izin/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIzinById(int id)
        {
            try
            {
                // TODO: Kullanıcının sadece kendi iznini veya yöneticisi ise altındaki personelin iznini görmesi kontrolü eklenebilir.
                var izin = await _izinService.GetByIdAsync(id);
                if (izin == null)
                {
                    return NotFound($"İzin with ID {id} not found.");
                }
                return Ok(izin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Izin
        [HttpPost]
        public async Task<IActionResult> CreateIzin([FromBody] IzinCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newIzinId = await _izinService.CreateAsync(dto);
                var createdIzin = await _izinService.GetByIdAsync(newIzinId);
                return CreatedAtAction(nameof(GetIzinById), new { id = newIzinId }, createdIzin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Izin/bekleyen
        [HttpGet("bekleyen")]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> GetBekleyenIzinler()
        {
            try
            {
                var bekleyenIzinler = await _izinService.GetBekleyenIzinlerAsync();
                return Ok(bekleyenIzinler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST: api/Izin/onayla
        [HttpPost("onayla")]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> OnaylaReddetIzin([FromBody] IzinOnayDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var onaylayanKullaniciId = GetCurrentUserId();
                await _izinService.OnaylaReddetAsync(dto.IzinId, dto.OnayDurumu, onaylayanKullaniciId, dto.RedNedeni);
                return Ok(new { message = "İzin durumu başarıyla güncellendi." });
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

        // DELETE: api/Izin/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> DeleteIzin(int id)
        {
            try
            {
                await _izinService.DeleteAsync(id);
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

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID could not be found in token.");
        }
    }
}