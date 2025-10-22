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
    public class SikayetController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public SikayetController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Sikayet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSikayetler()
        {
            var sikayetler = await _context.Sikayetler
                .Include(s => s.Personel)
                .Include(s => s.AtananKullanici)
                .ThenInclude(k => k.Personel)
                .Select(s => new
                {
                    s.Id,
                    PersonelAdi = s.Anonim ? "Anonim" : s.Personel.AdSoyad,
                    s.PersonelId,
                    s.Baslik,
                    s.Kategori,
                    s.OncelikSeviyesi,
                    s.Durum,
                    s.Anonim,
                    AtananKisi = s.AtananKullanici != null ? s.AtananKullanici.Personel.AdSoyad : null,
                    s.SikayetTarihi,
                    s.CozumTarihi
                })
                .OrderByDescending(s => s.SikayetTarihi)
                .ToListAsync();

            return Ok(sikayetler);
        }

        // GET: api/Sikayet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetSikayet(int id)
        {
            var sikayet = await _context.Sikayetler
                .Include(s => s.Personel)
                .Include(s => s.AtananKullanici)
                .ThenInclude(k => k.Personel)
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    PersonelAdi = s.Anonim ? "Anonim" : s.Personel.AdSoyad,
                    s.PersonelId,
                    s.Baslik,
                    s.Aciklama,
                    s.Kategori,
                    s.OncelikSeviyesi,
                    s.Durum,
                    s.Anonim,
                    s.EkDosyalar,
                    s.AtananKullaniciId,
                    AtananKisi = s.AtananKullanici != null ? s.AtananKullanici.Personel.AdSoyad : null,
                    s.CozumAciklamasi,
                    s.CozumTarihi,
                    s.SikayetTarihi
                })
                .FirstOrDefaultAsync();

            if (sikayet == null)
                return NotFound();

            return Ok(sikayet);
        }

        // POST: api/Sikayet
        [HttpPost]
        public async Task<ActionResult<Sikayet>> PostSikayet([FromBody] SikayetDTO dto)
        {
            var sikayet = new Sikayet
            {
                PersonelId = dto.PersonelId,
                Baslik = dto.Baslik,
                Aciklama = dto.Aciklama,
                Kategori = dto.Kategori,
                OncelikSeviyesi = dto.OncelikSeviyesi,
                Anonim = dto.Anonim,
                EkDosyalar = dto.EkDosyalar,
                SirketId = dto.SirketId
            };

            _context.Sikayetler.Add(sikayet);
            await _context.SaveChangesAsync();

            // İK'ya bildirim gönder
            await SendSikayetYoneticiBildirimi(sikayet.Id, sikayet.Baslik, sikayet.OncelikSeviyesi);

            return CreatedAtAction(nameof(GetSikayet), new { id = sikayet.Id }, sikayet);
        }

        // PUT: api/Sikayet/{id}/Ata
        [HttpPut("{id}/Ata")]
        public async Task<IActionResult> SikayetAta(int id, [FromBody] AtamaDTO dto)
        {
            var sikayet = await _context.Sikayetler.FindAsync(id);
            if (sikayet == null)
                return NotFound();

            sikayet.AtananKullaniciId = dto.AtananKullaniciId;
            sikayet.Durum = "İnceleniyor";

            await _context.SaveChangesAsync();

            // Atanan kişiye bildirim gönder
            await SendSikayetAtamaBildirimi(dto.AtananKullaniciId, sikayet.Baslik, sikayet.Id);

            return NoContent();
        }

        // PUT: api/Sikayet/{id}/Coz
        [HttpPut("{id}/Coz")]
        public async Task<IActionResult> SikayetCoz(int id, [FromBody] CozumDTO dto)
        {
            var sikayet = await _context.Sikayetler.FindAsync(id);
            if (sikayet == null)
                return NotFound();

            sikayet.Durum = "Çözüldü";
            sikayet.CozumAciklamasi = dto.CozumAciklamasi;
            sikayet.CozumTarihi = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Şikayet sahibine bildirim gönder (anonim değilse)
            if (!sikayet.Anonim)
            {
                await SendSikayetCozumBildirimi(sikayet.PersonelId, sikayet.Baslik);
            }

            return NoContent();
        }

        // PUT: api/Sikayet/{id}/Kapat
        [HttpPut("{id}/Kapat")]
        public async Task<IActionResult> SikayetKapat(int id)
        {
            var sikayet = await _context.Sikayetler.FindAsync(id);
            if (sikayet == null)
                return NotFound();

            sikayet.Durum = "Kapatıldı";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Sikayet/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelSikayetleri(int personelId)
        {
            var sikayetler = await _context.Sikayetler
                .Where(s => s.PersonelId == personelId)
                .OrderByDescending(s => s.SikayetTarihi)
                .Select(s => new
                {
                    s.Id,
                    s.Baslik,
                    s.Kategori,
                    s.OncelikSeviyesi,
                    s.Durum,
                    s.SikayetTarihi,
                    s.CozumTarihi
                })
                .ToListAsync();

            return Ok(sikayetler);
        }

        // GET: api/Sikayet/Acik
        [HttpGet("Acik")]
        public async Task<ActionResult<IEnumerable<object>>> GetAcikSikayetler()
        {
            var sikayetler = await _context.Sikayetler
                .Include(s => s.Personel)
                .Where(s => s.Durum == "Yeni" || s.Durum == "İnceleniyor")
                .OrderByDescending(s => s.OncelikSeviyesi == "Acil" ? 1 :
                                       s.OncelikSeviyesi == "Yüksek" ? 2 :
                                       s.OncelikSeviyesi == "Orta" ? 3 : 4)
                .ThenBy(s => s.SikayetTarihi)
                .Select(s => new
                {
                    s.Id,
                    PersonelAdi = s.Anonim ? "Anonim" : s.Personel.AdSoyad,
                    s.Baslik,
                    s.Kategori,
                    s.OncelikSeviyesi,
                    s.Durum,
                    s.SikayetTarihi,
                    GecenSure = (DateTime.UtcNow - s.SikayetTarihi).Days
                })
                .ToListAsync();

            return Ok(sikayetler);
        }

        // GET: api/Sikayet/Istatistik
        [HttpGet("Istatistik")]
        public async Task<ActionResult<object>> GetSikayetIstatistikleri()
        {
            var toplamSikayet = await _context.Sikayetler.CountAsync();
            var yeni = await _context.Sikayetler.CountAsync(s => s.Durum == "Yeni");
            var inceleniyor = await _context.Sikayetler.CountAsync(s => s.Durum == "İnceleniyor");
            var cozuldu = await _context.Sikayetler.CountAsync(s => s.Durum == "Çözüldü");
            var kapatildi = await _context.Sikayetler.CountAsync(s => s.Durum == "Kapatıldı");

            var kategoriDagilim = await _context.Sikayetler
                .GroupBy(s => s.Kategori)
                .Select(g => new
                {
                    Kategori = g.Key ?? "Diğer",
                    Sayi = g.Count()
                })
                .ToListAsync();

            var oncelikDagilim = await _context.Sikayetler
                .GroupBy(s => s.OncelikSeviyesi)
                .Select(g => new
                {
                    Oncelik = g.Key,
                    Sayi = g.Count()
                })
                .ToListAsync();

            // Ortalama çözüm süresi (gün)
            var cozulenler = await _context.Sikayetler
                .Where(s => s.CozumTarihi.HasValue)
                .Select(s => (s.CozumTarihi.Value - s.SikayetTarihi).TotalDays)
                .ToListAsync();

            var ortalamaCozumSuresi = cozulenler.Any() ? cozulenler.Average() : 0;

            return Ok(new
            {
                ToplamSikayet = toplamSikayet,
                Yeni = yeni,
                Inceleniyor = inceleniyor,
                Cozuldu = cozuldu,
                Kapatildi = kapatildi,
                KategoriDagilim = kategoriDagilim,
                OncelikDagilim = oncelikDagilim,
                OrtalamaCozumSuresi = Math.Round(ortalamaCozumSuresi, 1)
            });
        }

        // Helper Methods
        private async Task SendSikayetYoneticiBildirimi(int sikayetId, string baslik, string oncelik)
        {
            var ikKullaniciIds = await _context.Kullanicilar
                .Where(k => k.Aktif && k.Rol.RolAdi == "IK")
                .Select(k => k.Id)
                .ToListAsync();

            foreach (var kullaniciId in ikKullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = $"Yeni Şikayet ({oncelik})",
                    Mesaj = $"Yeni bir şikayet alındı: {baslik}",
                    Tip = oncelik == "Acil" ? "Uyarı" : "Bilgi",
                    ReferansTip = "Sikayet",
                    ReferansId = sikayetId
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SendSikayetAtamaBildirimi(int kullaniciId, string baslik, int sikayetId)
        {
            var bildirim = new Bildirim
            {
                KullaniciId = kullaniciId,
                Baslik = "Şikayet Atandı",
                Mesaj = $"Size bir şikayet atandı: {baslik}",
                Tip = "Bilgi",
                ReferansTip = "Sikayet",
                ReferansId = sikayetId
            };

            _context.Bildirimler.Add(bildirim);
            await _context.SaveChangesAsync();
        }

        private async Task SendSikayetCozumBildirimi(int personelId, string baslik)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == personelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = "Şikayet Çözüldü",
                    Mesaj = $"Şikayetiniz çözüldü: {baslik}",
                    Tip = "Başarı"
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs
    public class SikayetDTO
    {
        public int PersonelId { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string? Kategori { get; set; }
        public string OncelikSeviyesi { get; set; } = "Orta";
        public bool Anonim { get; set; }
        public string? EkDosyalar { get; set; }
        public int SirketId { get; set; }
    }

    public class AtamaDTO
    {
        public int AtananKullaniciId { get; set; }
    }

    public class CozumDTO
    {
        public string CozumAciklamasi { get; set; }
    }
}