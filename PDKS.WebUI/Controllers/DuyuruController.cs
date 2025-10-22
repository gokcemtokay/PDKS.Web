using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [ApiController]
    public class DuyuruController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public DuyuruController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Duyuru
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDuyurular()
        {
            var bugun = DateTime.UtcNow.Date;

            var duyurular = await _context.Duyurular
                .Include(d => d.OlusturanKullanici)
                .ThenInclude(k => k.Personel)
                .Where(d => d.Aktif &&
                           d.BaslangicTarihi <= bugun &&
                           (d.BitisTarihi == null || d.BitisTarihi >= bugun))
                .Select(d => new
                {
                    d.Id,
                    d.Baslik,
                    d.Icerik,
                    d.Tip,
                    d.BaslangicTarihi,
                    d.BitisTarihi,
                    d.AnaSayfadaGoster,
                    OlusturanKullanici = d.OlusturanKullanici.Personel.AdSoyad,
                    d.OlusturmaTarihi
                })
                .OrderByDescending(d => d.OlusturmaTarihi)
                .ToListAsync();

            return Ok(duyurular);
        }

        // GET: api/Duyuru/AnaSayfa
        [HttpGet("AnaSayfa")]
        public async Task<ActionResult<IEnumerable<object>>> GetAnaSayfaDuyurular()
        {
            var bugun = DateTime.UtcNow.Date;

            var duyurular = await _context.Duyurular
                .Where(d => d.Aktif &&
                           d.AnaSayfadaGoster &&
                           d.BaslangicTarihi <= bugun &&
                           (d.BitisTarihi == null || d.BitisTarihi >= bugun))
                .OrderByDescending(d => d.OlusturmaTarihi)
                .Take(5)
                .Select(d => new
                {
                    d.Id,
                    d.Baslik,
                    d.Icerik,
                    d.Tip,
                    d.OlusturmaTarihi
                })
                .ToListAsync();

            return Ok(duyurular);
        }

        // GET: api/Duyuru/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDuyuru(int id)
        {
            var duyuru = await _context.Duyurular
                .Include(d => d.OlusturanKullanici)
                .ThenInclude(k => k.Personel)
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.Baslik,
                    d.Icerik,
                    d.Tip,
                    d.BaslangicTarihi,
                    d.BitisTarihi,
                    d.Aktif,
                    d.AnaSayfadaGoster,
                    d.HedefDepartmanlar,
                    d.EkDosyalar,
                    OlusturanKullanici = d.OlusturanKullanici.Personel.AdSoyad,
                    d.OlusturmaTarihi
                })
                .FirstOrDefaultAsync();

            if (duyuru == null)
                return NotFound();

            return Ok(duyuru);
        }

        // POST: api/Duyuru
        [HttpPost]
        public async Task<ActionResult<Duyuru>> PostDuyuru([FromBody] DuyuruDTO dto)
        {
            var duyuru = new Duyuru
            {
                Baslik = dto.Baslik,
                Icerik = dto.Icerik,
                Tip = dto.Tip,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                AnaSayfadaGoster = dto.AnaSayfadaGoster,
                HedefDepartmanlar = dto.HedefDepartmanlar,
                EkDosyalar = dto.EkDosyalar,
                OlusturanKullaniciId = dto.OlusturanKullaniciId,
                SirketId = dto.SirketId
            };

            _context.Duyurular.Add(duyuru);
            await _context.SaveChangesAsync();

            // Hedef personellere bildirim gönder
            await SendDuyuruBildirimleri(duyuru);

            return CreatedAtAction(nameof(GetDuyuru), new { id = duyuru.Id }, duyuru);
        }

        // PUT: api/Duyuru/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDuyuru(int id, [FromBody] DuyuruDTO dto)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru == null)
                return NotFound();

            duyuru.Baslik = dto.Baslik;
            duyuru.Icerik = dto.Icerik;
            duyuru.Tip = dto.Tip;
            duyuru.BaslangicTarihi = dto.BaslangicTarihi;
            duyuru.BitisTarihi = dto.BitisTarihi;
            duyuru.Aktif = dto.Aktif;
            duyuru.AnaSayfadaGoster = dto.AnaSayfadaGoster;
            duyuru.HedefDepartmanlar = dto.HedefDepartmanlar;
            duyuru.EkDosyalar = dto.EkDosyalar;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Duyuru/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDuyuru(int id)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru == null)
                return NotFound();

            duyuru.Aktif = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper Methods
        private async Task SendDuyuruBildirimleri(Duyuru duyuru)
        {
            List<int> hedefKullaniciIds;

            if (string.IsNullOrEmpty(duyuru.HedefDepartmanlar))
            {
                // Tüm kullanıcılara gönder
                hedefKullaniciIds = await _context.Kullanicilar
                    .Where(k => k.Aktif)
                    .Select(k => k.Id)
                    .ToListAsync();
            }
            else
            {
                // Belirli departmanlara gönder (HedefDepartmanlar JSON array olarak geliyor)
                // Burada JSON parse edilmeli, şimdilik basit örnek
                hedefKullaniciIds = await _context.Kullanicilar
                    .Include(k => k.Personel)
                    .Where(k => k.Aktif)
                    .Select(k => k.Id)
                    .ToListAsync();
            }

            foreach (var kullaniciId in hedefKullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = $"Yeni Duyuru: {duyuru.Baslik}",
                    Mesaj = duyuru.Icerik.Length > 100 ? duyuru.Icerik.Substring(0, 100) + "..." : duyuru.Icerik,
                    Tip = "Bilgi",
                    ReferansTip = "Duyuru",
                    ReferansId = duyuru.Id
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();
        }
    }

    // DTO
    public class DuyuruDTO
    {
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string? Tip { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public bool Aktif { get; set; } = true;
        public bool AnaSayfadaGoster { get; set; } = true;
        public string? HedefDepartmanlar { get; set; }
        public string? EkDosyalar { get; set; }
        public int OlusturanKullaniciId { get; set; }
        public int SirketId { get; set; }
    }
}