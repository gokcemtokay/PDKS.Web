// PDKS.WebUI/Controllers/OnayAkisiController.cs - DÜZELTİLMİŞ

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.Services;
using PDKS.Business.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OnayAkisiController : ControllerBase
    {
        private readonly IOnayAkisiService _onayAkisiService;

        public OnayAkisiController(IOnayAkisiService onayAkisiService)
        {
            _onayAkisiService = onayAkisiService;
        }

        // GET: api/OnayAkisi/sirket/{sirketId}
        [HttpGet("sirket/{sirketId}")]
        public async Task<IActionResult> GetBySirket(int sirketId)
        {
            var akislar = await _onayAkisiService.GetAllOnayAkislariAsync(sirketId);
            return Ok(akislar);
        }

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

        // GET: api/OnayAkisi/bekleyen
        [HttpGet("bekleyen")]
        public async Task<IActionResult> GetBekleyenOnaylar()
        {
            // ❌ ESKİ - Hatalı
            // var kullaniciId = int.Parse(User.FindFirst("sub")?.Value ?? "0");

            // ✅ YENİ - Doğru claim okuma
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                                ?? User.FindFirst("sub")
                                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (kullaniciIdClaim == null || string.IsNullOrEmpty(kullaniciIdClaim.Value))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });
            }

            if (!int.TryParse(kullaniciIdClaim.Value, out int kullaniciId) || kullaniciId <= 0)
            {
                return BadRequest(new { message = "Geçersiz kullanıcı kimliği" });
            }

            var bekleyenler = await _onayAkisiService.GetBekleyenOnaylarAsync(kullaniciId);
            return Ok(bekleyenler);
        }

        // POST: api/OnayAkisi/onayla
        [HttpPost("onayla")]
        public async Task<IActionResult> Onayla([FromBody] OnayIslemDTO dto)
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                                ?? User.FindFirst("sub");

            if (kullaniciIdClaim == null || !int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });
            }

            await _onayAkisiService.OnaylaAsync(dto.OnayKaydiId, kullaniciId, dto.Aciklama);
            return Ok(new { message = "Onaylandı" });
        }

        // POST: api/OnayAkisi/reddet
        [HttpPost("reddet")]
        public async Task<IActionResult> Reddet([FromBody] OnayIslemDTO dto)
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                                ?? User.FindFirst("sub");

            if (kullaniciIdClaim == null || !int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });
            }

            await _onayAkisiService.ReddetAsync(dto.OnayKaydiId, kullaniciId, dto.Aciklama);
            return Ok(new { message = "Reddedildi" });
        }

        // GET: api/OnayAkisi/taleplerim
        [HttpGet("taleplerim")]
        public async Task<IActionResult> GetTaleplerim()
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                                ?? User.FindFirst("sub");

            if (kullaniciIdClaim == null || !int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği bulunamadı" });
            }

            var talepler = await _onayAkisiService.GetKullaniciTaleplerAsync(kullaniciId);
            return Ok(talepler);
        }
    }
}