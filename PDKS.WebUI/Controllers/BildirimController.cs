using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;
using PDKS.WebUI.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BildirimController : ControllerBase
    {
        private readonly PDKSDbContext _context;
        private readonly IBildirimService _bildirimService;

        public BildirimController(PDKSDbContext context, IBildirimService bildirimService)
        {
            _context = context;
            _bildirimService = bildirimService;
        }

        // GET: api/Bildirim/Kullanici/{kullaniciId}
        [HttpGet("Kullanici/{kullaniciId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetKullaniciBildirimler(int kullaniciId)
        {
            var bildirimler = await _context.Bildirimler
                .Where(b => b.KullaniciId == kullaniciId)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .Select(b => new
                {
                    b.Id,
                    b.Baslik,
                    b.Mesaj,
                    b.Tip,
                    b.Okundu,
                    b.OkunmaTarihi,
                    b.Link,
                    b.ReferansTip,
                    b.ReferansId,
                    b.OlusturmaTarihi
                })
                .ToListAsync();

            return Ok(bildirimler);
        }

        // GET: api/Bildirim/Okunmamis/{kullaniciId}
        [HttpGet("Okunmamis/{kullaniciId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOkunmamisBildirimler(int kullaniciId)
        {
            var bildirimler = await _context.Bildirimler
                .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .Select(b => new
                {
                    b.Id,
                    b.Baslik,
                    b.Mesaj,
                    b.Tip,
                    b.Link,
                    b.ReferansTip,
                    b.ReferansId,
                    b.OlusturmaTarihi
                })
                .ToListAsync();

            return Ok(bildirimler);
        }

        // GET: api/Bildirim/OkunmamisSayisi/{kullaniciId}
        [HttpGet("OkunmamisSayisi/{kullaniciId}")]
        public async Task<ActionResult<int>> GetOkunmamisSayisi(int kullaniciId)
        {
            var sayi = await _context.Bildirimler
                .CountAsync(b => b.KullaniciId == kullaniciId && !b.Okundu);

            return Ok(sayi);
        }

        // PUT: api/Bildirim/Okundu/{id}
        [HttpPut("Okundu/{id}")]
        public async Task<IActionResult> OkunduIsaretle(int id)
        {
            var bildirim = await _context.Bildirimler.FindAsync(id);
            if (bildirim == null)
                return NotFound();

            bildirim.Okundu = true;
            bildirim.OkunmaTarihi = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Bildirim/TumunuOkundu/{kullaniciId}
        [HttpPut("TumunuOkundu/{kullaniciId}")]
        public async Task<IActionResult> TumunuOkunduIsaretle(int kullaniciId)
        {
            var bildirimler = await _context.Bildirimler
                .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                .ToListAsync();

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
                bildirim.OkunmaTarihi = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Bildirim/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBildirim(int id)
        {
            var bildirim = await _context.Bildirimler.FindAsync(id);
            if (bildirim == null)
                return NotFound();

            _context.Bildirimler.Remove(bildirim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Bildirim/Temizle/{kullaniciId}
        [HttpDelete("Temizle/{kullaniciId}")]
        public async Task<IActionResult> TumBildirimleriTemizle(int kullaniciId)
        {
            var bildirimler = await _context.Bildirimler
                .Where(b => b.KullaniciId == kullaniciId && b.Okundu)
                .ToListAsync();

            _context.Bildirimler.RemoveRange(bildirimler);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Bildirim
        [HttpPost]
        public async Task<ActionResult<Bildirim>> PostBildirim([FromBody] BildirimDTO dto)
        {
            var bildirim = new Bildirim
            {
                KullaniciId = dto.KullaniciId,
                Baslik = dto.Baslik,
                Mesaj = dto.Mesaj,
                Tip = dto.Tip,
                Link = dto.Link,
                ReferansTip = dto.ReferansTip,
                ReferansId = dto.ReferansId
            };

            _context.Bildirimler.Add(bildirim);
            await _context.SaveChangesAsync();

            return Ok(bildirim);
        }

        // POST: api/Bildirim/Toplu
        [HttpPost("Toplu")]
        public async Task<ActionResult> TopluBildirimGonder([FromBody] TopluBildirimDTO dto)
        {
            // Hedef kullanıcıları al
            var kullaniciIds = dto.KullaniciIds;

            if (dto.TumPersonel)
            {
                kullaniciIds = await _context.Kullanicilar
                    .Where(k => k.Aktif)
                    .Select(k => k.Id)
                    .ToListAsync();
            }
            else if (dto.DepartmanId.HasValue)
            {
                kullaniciIds = await _context.Kullanicilar
                    .Include(k => k.Personel)
                    .Where(k => k.Aktif && k.Personel.DepartmanId == dto.DepartmanId)
                    .Select(k => k.Id)
                    .ToListAsync();
            }

            foreach (var kullaniciId in kullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = dto.Baslik,
                    Mesaj = dto.Mesaj,
                    Tip = dto.Tip
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{kullaniciIds.Count} kullanıcıya bildirim gönderildi." });
        }
    }

    // DTOs
    public class BildirimDTO
    {
        public int KullaniciId { get; set; }
        public string Baslik { get; set; }
        public string? Mesaj { get; set; }
        public string? Tip { get; set; }
        public string? Link { get; set; }
        public string? ReferansTip { get; set; }
        public int? ReferansId { get; set; }
    }

    public class TopluBildirimDTO
    {
        public string Baslik { get; set; }
        public string Mesaj { get; set; }
        public string? Tip { get; set; }
        public bool TumPersonel { get; set; }
        public int? DepartmanId { get; set; }
        public List<int>? KullaniciIds { get; set; }
    }
}