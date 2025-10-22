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
    public class EtkinlikController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public EtkinlikController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Etkinlik
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEtkinlikler()
        {
            var etkinlikler = await _context.Etkinlikler
                .Include(e => e.DuzenleyenKullanici)
                .ThenInclude(k => k.Personel)
                .Include(e => e.Katilimcilar)
                .Select(e => new
                {
                    e.Id,
                    e.EtkinlikAdi,
                    e.Aciklama,
                    e.EtkinlikTipi,
                    e.BaslangicTarihi,
                    e.BitisTarihi,
                    e.Konum,
                    e.KontenjanSayisi,
                    e.KatilimZorunlu,
                    DuzenleyenKullanici = e.DuzenleyenKullanici.Personel.AdSoyad,
                    e.OlusturmaTarihi,
                    KatilimciSayisi = e.Katilimcilar.Count(k => k.KatilimDurumu == "Katilacak" || k.KatilimDurumu == "Katildi"),
                    MusaitKontenjan = e.KontenjanSayisi.HasValue ?
                        e.KontenjanSayisi.Value - e.Katilimcilar.Count(k => k.KatilimDurumu == "Katilacak" || k.KatilimDurumu == "Katildi") :
                        (int?)null
                })
                .OrderBy(e => e.BaslangicTarihi)
                .ToListAsync();

            return Ok(etkinlikler);
        }

        // GET: api/Etkinlik/Gelecek
        [HttpGet("Gelecek")]
        public async Task<ActionResult<IEnumerable<object>>> GetGelecekEtkinlikler()
        {
            var bugun = DateTime.UtcNow;

            var etkinlikler = await _context.Etkinlikler
                .Include(e => e.DuzenleyenKullanici)
                .ThenInclude(k => k.Personel)
                .Include(e => e.Katilimcilar)
                .Where(e => e.BaslangicTarihi >= bugun)
                .Select(e => new
                {
                    e.Id,
                    e.EtkinlikAdi,
                    e.EtkinlikTipi,
                    e.BaslangicTarihi,
                    e.BitisTarihi,
                    e.Konum,
                    KatilimciSayisi = e.Katilimcilar.Count(k => k.KatilimDurumu == "Katilacak"),
                    e.KontenjanSayisi
                })
                .OrderBy(e => e.BaslangicTarihi)
                .Take(10)
                .ToListAsync();

            return Ok(etkinlikler);
        }

        // GET: api/Etkinlik/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEtkinlik(int id)
        {
            var etkinlik = await _context.Etkinlikler
                .Include(e => e.DuzenleyenKullanici)
                .ThenInclude(k => k.Personel)
                .Include(e => e.Katilimcilar)
                .ThenInclude(k => k.Personel)
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.EtkinlikAdi,
                    e.Aciklama,
                    e.EtkinlikTipi,
                    e.BaslangicTarihi,
                    e.BitisTarihi,
                    e.Konum,
                    e.KontenjanSayisi,
                    e.KatilimZorunlu,
                    e.HedefKatilimcilar,
                    e.KapakResmi,
                    DuzenleyenKullanici = e.DuzenleyenKullanici.Personel.AdSoyad,
                    e.DuzenleyenKullaniciId,
                    e.OlusturmaTarihi,
                    Katilimcilar = e.Katilimcilar.Select(k => new
                    {
                        k.Id,
                        k.PersonelId,
                        PersonelAdi = k.Personel.AdSoyad,
                        k.KatilimDurumu,
                        k.KayitTarihi,
                        k.Not
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (etkinlik == null)
                return NotFound();

            return Ok(etkinlik);
        }

        // POST: api/Etkinlik
        [HttpPost]
        public async Task<ActionResult<Etkinlik>> PostEtkinlik([FromBody] EtkinlikDTO dto)
        {
            var etkinlik = new Etkinlik
            {
                EtkinlikAdi = dto.EtkinlikAdi,
                Aciklama = dto.Aciklama,
                EtkinlikTipi = dto.EtkinlikTipi,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                Konum = dto.Konum,
                KontenjanSayisi = dto.KontenjanSayisi,
                KatilimZorunlu = dto.KatilimZorunlu,
                HedefKatilimcilar = dto.HedefKatilimcilar,
                KapakResmi = dto.KapakResmi,
                DuzenleyenKullaniciId = dto.DuzenleyenKullaniciId,
                SirketId = dto.SirketId
            };

            _context.Etkinlikler.Add(etkinlik);
            await _context.SaveChangesAsync();

            // Hedef katılımcılara bildirim gönder
            await SendEtkinlikBildirimleri(etkinlik);

            return CreatedAtAction(nameof(GetEtkinlik), new { id = etkinlik.Id }, etkinlik);
        }

        // PUT: api/Etkinlik/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEtkinlik(int id, [FromBody] EtkinlikDTO dto)
        {
            var etkinlik = await _context.Etkinlikler.FindAsync(id);
            if (etkinlik == null)
                return NotFound();

            etkinlik.EtkinlikAdi = dto.EtkinlikAdi;
            etkinlik.Aciklama = dto.Aciklama;
            etkinlik.EtkinlikTipi = dto.EtkinlikTipi;
            etkinlik.BaslangicTarihi = dto.BaslangicTarihi;
            etkinlik.BitisTarihi = dto.BitisTarihi;
            etkinlik.Konum = dto.Konum;
            etkinlik.KontenjanSayisi = dto.KontenjanSayisi;
            etkinlik.KatilimZorunlu = dto.KatilimZorunlu;
            etkinlik.HedefKatilimcilar = dto.HedefKatilimcilar;
            etkinlik.KapakResmi = dto.KapakResmi;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Etkinlik/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEtkinlik(int id)
        {
            var etkinlik = await _context.Etkinlikler.FindAsync(id);
            if (etkinlik == null)
                return NotFound();

            _context.Etkinlikler.Remove(etkinlik);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Etkinlik/{id}/Katil
        [HttpPost("{id}/Katil")]
        public async Task<ActionResult> KatilimBelirt(int id, [FromBody] KatilimDTO dto)
        {
            var etkinlik = await _context.Etkinlikler
                .Include(e => e.Katilimcilar)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (etkinlik == null)
                return NotFound();

            // Kontenjan kontrolü
            if (etkinlik.KontenjanSayisi.HasValue)
            {
                var mevcutKatilimciSayisi = etkinlik.Katilimcilar
                    .Count(k => k.KatilimDurumu == "Katilacak" || k.KatilimDurumu == "Katildi");

                if (mevcutKatilimciSayisi >= etkinlik.KontenjanSayisi.Value)
                    return BadRequest("Kontenjan dolu.");
            }

            // Daha önce katılım var mı kontrol et
            var mevcutKatilim = await _context.EtkinlikKatilimcilari
                .FirstOrDefaultAsync(k => k.EtkinlikId == id && k.PersonelId == dto.PersonelId);

            if (mevcutKatilim != null)
            {
                // Mevcut katılım durumunu güncelle
                mevcutKatilim.KatilimDurumu = dto.KatilimDurumu;
                mevcutKatilim.Not = dto.Not;
            }
            else
            {
                // Yeni katılım oluştur
                var katilim = new EtkinlikKatilimci
                {
                    EtkinlikId = id,
                    PersonelId = dto.PersonelId,
                    KatilimDurumu = dto.KatilimDurumu,
                    Not = dto.Not
                };

                _context.EtkinlikKatilimcilari.Add(katilim);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Katılım durumu kaydedildi." });
        }

        // GET: api/Etkinlik/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelEtkinlikleri(int personelId)
        {
            var etkinlikler = await _context.EtkinlikKatilimcilari
                .Include(k => k.Etkinlik)
                .Where(k => k.PersonelId == personelId)
                .Select(k => new
                {
                    k.EtkinlikId,
                    k.Etkinlik.EtkinlikAdi,
                    k.Etkinlik.EtkinlikTipi,
                    k.Etkinlik.BaslangicTarihi,
                    k.Etkinlik.BitisTarihi,
                    k.Etkinlik.Konum,
                    k.KatilimDurumu,
                    k.KayitTarihi
                })
                .OrderBy(e => e.BaslangicTarihi)
                .ToListAsync();

            return Ok(etkinlikler);
        }

        // Helper Methods
        private async Task SendEtkinlikBildirimleri(Etkinlik etkinlik)
        {
            List<int> hedefKullaniciIds;

            if (string.IsNullOrEmpty(etkinlik.HedefKatilimcilar))
            {
                // Tüm kullanıcılara gönder
                hedefKullaniciIds = await _context.Kullanicilar
                    .Where(k => k.Aktif)
                    .Select(k => k.Id)
                    .ToListAsync();
            }
            else
            {
                // Belirli hedef gruplara gönder
                hedefKullaniciIds = await _context.Kullanicilar
                    .Where(k => k.Aktif)
                    .Select(k => k.Id)
                    .ToListAsync();
            }

            foreach (var kullaniciId in hedefKullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = $"Yeni Etkinlik: {etkinlik.EtkinlikAdi}",
                    Mesaj = $"{etkinlik.BaslangicTarihi:dd.MM.yyyy} tarihinde {etkinlik.EtkinlikTipi} etkinliği düzenleniyor.",
                    Tip = "Bilgi",
                    ReferansTip = "Etkinlik",
                    ReferansId = etkinlik.Id
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();
        }
    }

    // DTOs
    public class EtkinlikDTO
    {
        public string EtkinlikAdi { get; set; }
        public string? Aciklama { get; set; }
        public string EtkinlikTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Konum { get; set; }
        public int? KontenjanSayisi { get; set; }
        public bool KatilimZorunlu { get; set; }
        public string? HedefKatilimcilar { get; set; }
        public string? KapakResmi { get; set; }
        public int DuzenleyenKullaniciId { get; set; }
        public int SirketId { get; set; }
    }

    public class KatilimDTO
    {
        public int PersonelId { get; set; }
        public string KatilimDurumu { get; set; } // Beklemede, Katilacak, Katilmayacak, Katildi
        public string? Not { get; set; }
    }
}