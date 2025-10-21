using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OneriController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public OneriController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Oneri
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOneriler()
        {
            var oneriler = await _context.Oneriler
                .Include(o => o.Personel)
                .Include(o => o.DegerlendirenKullanici)
                .ThenInclude(k => k.Personel)
                .Select(o => new
                {
                    o.Id,
                    PersonelAdi = o.Anonim ? "Anonim" : o.Personel.AdSoyad,
                    o.PersonelId,
                    o.Baslik,
                    o.Aciklama,
                    o.Kategori,
                    o.Durum,
                    o.Anonim,
                    o.DegerlendirmePuani,
                    DegerlendirenKullanici = o.DegerlendirenKullanici != null ? o.DegerlendirenKullanici.Personel.AdSoyad : null,
                    o.OneriTarihi,
                    o.OdulMiktari
                })
                .OrderByDescending(o => o.OneriTarihi)
                .ToListAsync();

            return Ok(oneriler);
        }

        // GET: api/Oneri/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOneri(int id)
        {
            var oneri = await _context.Oneriler
                .Include(o => o.Personel)
                .Include(o => o.DegerlendirenKullanici)
                .ThenInclude(k => k.Personel)
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    PersonelAdi = o.Anonim ? "Anonim" : o.Personel.AdSoyad,
                    o.PersonelId,
                    o.Baslik,
                    o.Aciklama,
                    o.Kategori,
                    o.Durum,
                    o.Anonim,
                    o.EkDosyalar,
                    o.DegerlendirmePuani,
                    o.DegerlendirmeNotu,
                    DegerlendirenKullanici = o.DegerlendirenKullanici != null ? o.DegerlendirenKullanici.Personel.AdSoyad : null,
                    o.DegerlendirmeTarihi,
                    o.OdulMiktari,
                    o.OneriTarihi
                })
                .FirstOrDefaultAsync();

            if (oneri == null)
                return NotFound();

            return Ok(oneri);
        }

        // POST: api/Oneri
        [HttpPost]
        public async Task<ActionResult<Oneri>> PostOneri([FromBody] OneriDTO dto)
        {
            var oneri = new Oneri
            {
                PersonelId = dto.PersonelId,
                Baslik = dto.Baslik,
                Aciklama = dto.Aciklama,
                Kategori = dto.Kategori,
                Anonim = dto.Anonim,
                EkDosyalar = dto.EkDosyalar,
                SirketId = dto.SirketId
            };

            _context.Oneriler.Add(oneri);
            await _context.SaveChangesAsync();

            // İK'ya bildirim gönder
            await SendOneriYoneticiBildirimi(oneri.Id, oneri.Baslik);

            return CreatedAtAction(nameof(GetOneri), new { id = oneri.Id }, oneri);
        }

        // PUT: api/Oneri/{id}/Degerlendir
        [HttpPut("{id}/Degerlendir")]
        public async Task<IActionResult> OneriDegerlendir(int id, [FromBody] DegerlendirmeDTO dto)
        {
            var oneri = await _context.Oneriler.FindAsync(id);
            if (oneri == null)
                return NotFound();

            oneri.Durum = dto.Durum;
            oneri.DegerlendirmePuani = dto.DegerlendirmePuani;
            oneri.DegerlendirmeNotu = dto.DegerlendirmeNotu;
            oneri.DegerlendirenKullaniciId = dto.DegerlendirenKullaniciId;
            oneri.DegerlendirmeTarihi = DateTime.UtcNow;
            oneri.OdulMiktari = dto.OdulMiktari;

            await _context.SaveChangesAsync();

            // Öneri sahibine bildirim gönder
            await SendOneriSonucBildirimi(oneri.PersonelId, oneri.Baslik, dto.Durum);

            return NoContent();
        }

        // GET: api/Oneri/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelOnerileri(int personelId)
        {
            var oneriler = await _context.Oneriler
                .Where(o => o.PersonelId == personelId)
                .OrderByDescending(o => o.OneriTarihi)
                .Select(o => new
                {
                    o.Id,
                    o.Baslik,
                    o.Kategori,
                    o.Durum,
                    o.OneriTarihi,
                    o.DegerlendirmePuani,
                    o.OdulMiktari
                })
                .ToListAsync();

            return Ok(oneriler);
        }

        // GET: api/Oneri/Bekleyen
        [HttpGet("Bekleyen")]
        public async Task<ActionResult<IEnumerable<object>>> GetBekleyenOneriler()
        {
            var oneriler = await _context.Oneriler
                .Include(o => o.Personel)
                .Where(o => o.Durum == "Beklemede" || o.Durum == "İnceleniyor")
                .OrderBy(o => o.OneriTarihi)
                .Select(o => new
                {
                    o.Id,
                    PersonelAdi = o.Anonim ? "Anonim" : o.Personel.AdSoyad,
                    o.Baslik,
                    o.Kategori,
                    o.Durum,
                    o.OneriTarihi
                })
                .ToListAsync();

            return Ok(oneriler);
        }

        // GET: api/Oneri/Istatistik
        [HttpGet("Istatistik")]
        public async Task<ActionResult<object>> GetOneriIstatistikleri()
        {
            var toplamOneri = await _context.Oneriler.CountAsync();
            var bekleyen = await _context.Oneriler.CountAsync(o => o.Durum == "Beklemede");
            var inceleniyor = await _context.Oneriler.CountAsync(o => o.Durum == "İnceleniyor");
            var kabulEdilen = await _context.Oneriler.CountAsync(o => o.Durum == "Kabul Edildi");
            var uygulanan = await _context.Oneriler.CountAsync(o => o.Durum == "Uygulandi");
            var reddedilen = await _context.Oneriler.CountAsync(o => o.Durum == "Reddedildi");

            var kategoriDagilim = await _context.Oneriler
                .GroupBy(o => o.Kategori)
                .Select(g => new
                {
                    Kategori = g.Key ?? "Diğer",
                    Sayi = g.Count()
                })
                .ToListAsync();

            return Ok(new
            {
                ToplamOneri = toplamOneri,
                Bekleyen = bekleyen,
                Inceleniyor = inceleniyor,
                KabulEdilen = kabulEdilen,
                Uygulanan = uygulanan,
                Reddedilen = reddedilen,
                KategoriDagilim = kategoriDagilim
            });
        }

        // Helper Methods
        private async Task SendOneriYoneticiBildirimi(int oneriId, string oneriBaslik)
        {
            // İK rolündeki kullanıcılara bildirim gönder
            var ikKullaniciIds = await _context.Kullanicilar
                .Where(k => k.Aktif && k.Rol.RolAdi == "IK")
                .Select(k => k.Id)
                .ToListAsync();

            foreach (var kullaniciId in ikKullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = "Yeni Öneri",
                    Mesaj = $"Yeni bir öneri alındı: {oneriBaslik}",
                    Tip = "Bilgi",
                    ReferansTip = "Oneri",
                    ReferansId = oneriId
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SendOneriSonucBildirimi(int personelId, string oneriBaslik, string durum)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == personelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = "Öneri Değerlendirmesi",
                    Mesaj = $"'{oneriBaslik}' öneriniz değerlendirildi. Durum: {durum}",
                    Tip = durum == "Kabul Edildi" || durum == "Uygulandi" ? "Başarı" : "Bilgi"
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs
    public class OneriDTO
    {
        public int PersonelId { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string? Kategori { get; set; }
        public bool Anonim { get; set; }
        public string? EkDosyalar { get; set; }
        public int SirketId { get; set; }
    }

    public class DegerlendirmeDTO
    {
        public string Durum { get; set; } // Beklemede, İnceleniyor, Kabul Edildi, Reddedildi, Uygulandi
        public int? DegerlendirmePuani { get; set; }
        public string? DegerlendirmeNotu { get; set; }
        public int DegerlendirenKullaniciId { get; set; }
        public decimal? OdulMiktari { get; set; }
    }
}