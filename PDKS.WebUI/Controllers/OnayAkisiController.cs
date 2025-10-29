// PDKS.WebUI/Controllers/OnayAkisiController.cs - ROUTE SIRASI DÜZELTİLDİ

using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.Services;
using PDKS.Business.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
#if DEBUG
    [Route("api/[controller]")]
#else
    [Route("[controller]")]
#endif
    [ApiController]
    public class OnayAkisiController : ControllerBase
    {
        private readonly IOnayAkisiService _onayAkisiService;

        public OnayAkisiController(IOnayAkisiService onayAkisiService)
        {
            _onayAkisiService = onayAkisiService;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        // Helper method: Kullanıcı ID'sini token'dan al
        private IActionResult GetKullaniciIdFromToken(out int kullaniciId)
        {
            kullaniciId = 0;

            var kullaniciIdClaim = User.FindFirst("sub")
                                ?? User.FindFirst(ClaimTypes.NameIdentifier)
                                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (kullaniciIdClaim == null || string.IsNullOrEmpty(kullaniciIdClaim.Value))
            {
                var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Unauthorized(new
                {
                    message = "Kullanıcı kimliği bulunamadı",
                    availableClaims = allClaims
                });
            }

            if (!int.TryParse(kullaniciIdClaim.Value, out kullaniciId) || kullaniciId <= 0)
            {
                return BadRequest(new
                {
                    message = "Geçersiz kullanıcı kimliği",
                    claimType = kullaniciIdClaim.Type,
                    claimValue = kullaniciIdClaim.Value
                });
            }

            return null; // Success
        }

        // ✅ SPESİFİK ROUTE'LAR ÖNCE GELMELİ

        // GET: api/OnayAkisi/bekleyen
        [HttpGet("bekleyen")]
        public async Task<IActionResult> GetBekleyenOnaylar()
        {
            var errorResult = GetKullaniciIdFromToken(out int kullaniciId);
            if (errorResult != null)
                return errorResult;

            var bekleyenler = await _onayAkisiService.GetBekleyenOnaylarAsync(kullaniciId);
            return Ok(bekleyenler);
        }

        // GET: api/OnayAkisi/taleplerim
        [HttpGet("taleplerim")]
        public async Task<IActionResult> GetTaleplerim()
        {
            var errorResult = GetKullaniciIdFromToken(out int kullaniciId);
            if (errorResult != null)
                return errorResult;

            var talepler = await _onayAkisiService.GetKullaniciTaleplerAsync(kullaniciId);
            return Ok(talepler);
        }

        // GET: api/OnayAkisi/sirket/{sirketId}
        [HttpGet("sirket/{sirketId}")]
        public async Task<IActionResult> GetBySirket(int sirketId)
        {
            var akislar = await _onayAkisiService.GetAllOnayAkislariAsync(sirketId);
            return Ok(akislar);
        }

        // ✅ GENERİK ROUTE'LAR EN SONA

        // GET: api/OnayAkisi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var akis = await _onayAkisiService.GetOnayAkisiByIdAsync(id);
            if (akis == null)
                return NotFound();
            return Ok(akis);
        }

        // POST: api/OnayAkisi
        [HttpPost]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Create([FromBody] OnayAkisiDTO dto)
        {
            var akis = await _onayAkisiService.CreateOnayAkisiAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = akis.Id }, akis);
        }

        // PUT: api/OnayAkisi/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Update(int id, [FromBody] OnayAkisiDTO dto)
        {
            var akis = await _onayAkisiService.UpdateOnayAkisiAsync(id, dto);
            return Ok(akis);
        }

        // DELETE: api/OnayAkisi/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Delete(int id)
        {
            await _onayAkisiService.DeleteOnayAkisiAsync(id);
            return NoContent();
        }

        // POST: api/OnayAkisi/onayla
        [HttpPost("onayla")]
        public async Task<IActionResult> Onayla([FromBody] OnayIslemDTO dto)
        {
            var errorResult = GetKullaniciIdFromToken(out int kullaniciId);
            if (errorResult != null)
                return errorResult;

            await _onayAkisiService.OnaylaAsync(dto.OnayKaydiId, kullaniciId, dto.Aciklama);
            return Ok(new { message = "Onaylandı" });
        }

        // POST: api/OnayAkisi/reddet
        [HttpPost("reddet")]
        public async Task<IActionResult> Reddet([FromBody] OnayIslemDTO dto)
        {
            var errorResult = GetKullaniciIdFromToken(out int kullaniciId);
            if (errorResult != null)
                return errorResult;

            await _onayAkisiService.ReddetAsync(dto.OnayKaydiId, kullaniciId, dto.Aciklama);
            return Ok(new { message = "Reddedildi" });
        }
    }
}