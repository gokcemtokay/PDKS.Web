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
    public class MasrafTalebiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public MasrafTalebiController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/MasrafTalebi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetMasrafTalepleri()
        {
            var talepler = await _context.MasrafTalepleri
                .Include(m => m.Personel)
                .Select(m => new
                {
                    m.Id,
                    PersonelAdi = m.Personel.AdSoyad,
                    m.PersonelId,
                    m.MasrafTipi,
                    m.Tutar,
                    m.Tarih,
                    m.Aciklama,
                    m.OnayDurumu,
                    m.TalepTarihi,
                    m.OdemeTarihi
                })
                .OrderByDescending(m => m.TalepTarihi)
                .ToListAsync();

            return Ok(talepler);
        }

        // GET: api/MasrafTalebi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMasrafTalebi(int id)
        {
            var talep = await _context.MasrafTalepleri
                .Include(m => m.Personel)
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    m.Id,
                    PersonelAdi = m.Personel.AdSoyad,
                    m.PersonelId,
                    m.MasrafTipi,
                    m.Tutar,
                    m.Tarih,
                    m.Aciklama,
                    m.Faturalar,
                    m.KdvOrani,
                    m.KdvTutari,
                    ToplamTutar = m.Tutar + (m.KdvTutari ?? 0),
                    m.OnayDurumu,
                    m.TalepTarihi,
                    m.OdemeTarihi,
                    OnayAkisi = _context.OnayAkislari
                        .Where(o => o.OnayTipi == "Masraf" && o.ReferansId == m.Id)
                        .Select(o => new
                        {
                            o.Sira,
                            OnaylayiciAdi = o.Onaylayici.AdSoyad,
                            o.OnayDurumu,
                            o.OnayTarihi,
                            o.Aciklama
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (talep == null)
                return NotFound();

            return Ok(talep);
        }

        // POST: api/MasrafTalebi
        [HttpPost]
        public async Task<ActionResult<MasrafTalebi>> PostMasrafTalebi([FromBody] MasrafTalebiDTO dto)
        {
            // KDV hesaplama
            decimal? kdvTutari = null;
            if (dto.KdvOrani.HasValue)
            {
                kdvTutari = dto.Tutar * (dto.KdvOrani.Value / 100);
            }

            var masrafTalebi = new MasrafTalebi
            {
                PersonelId = dto.PersonelId,
                MasrafTipi = dto.MasrafTipi,
                Tutar = dto.Tutar,
                Tarih = dto.Tarih,
                Aciklama = dto.Aciklama,
                Faturalar = dto.Faturalar,
                KdvOrani = dto.KdvOrani,
                KdvTutari = kdvTutari,
                SirketId = dto.SirketId
            };

            _context.MasrafTalepleri.Add(masrafTalebi);
            await _context.SaveChangesAsync();

            // Onay akışı oluştur
            if (dto.OnaylayiciIds != null && dto.OnaylayiciIds.Any())
            {
                for (int i = 0; i < dto.OnaylayiciIds.Count; i++)
                {
                    var onayAkisi = new OnayAkisi
                    {
                        OnayTipi = "Masraf",
                        ReferansId = masrafTalebi.Id,
                        Sira = i + 1,
                        OnaylayiciPersonelId = dto.OnaylayiciIds[i],
                        SirketId = dto.SirketId
                    };
                    _context.OnayAkislari.Add(onayAkisi);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetMasrafTalebi), new { id = masrafTalebi.Id }, masrafTalebi);
        }

        // PUT: api/MasrafTalebi/Odemesi/{id}
        [HttpPut("Odemesi/{id}")]
        public async Task<IActionResult> OdemesiYapildi(int id)
        {
            var talep = await _context.MasrafTalepleri.FindAsync(id);
            if (talep == null)
                return NotFound();

            if (talep.OnayDurumu != "Onaylandi")
                return BadRequest("Sadece onaylanmış masrafların ödemesi yapılabilir.");

            talep.OnayDurumu = "Odendi";
            talep.OdemeTarihi = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Personele bildirim gönder
            await SendMasrafBildirimi(talep.PersonelId, "Masraf ödemesi yapıldı", talep.Id);

            return NoContent();
        }

        // GET: api/MasrafTalebi/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelMasrafTalepleri(int personelId)
        {
            var talepler = await _context.MasrafTalepleri
                .Where(m => m.PersonelId == personelId)
                .OrderByDescending(m => m.TalepTarihi)
                .Select(m => new
                {
                    m.Id,
                    m.MasrafTipi,
                    m.Tutar,
                    m.Tarih,
                    m.OnayDurumu,
                    m.TalepTarihi,
                    m.OdemeTarihi
                })
                .ToListAsync();

            return Ok(talepler);
        }

        // GET: api/MasrafTalebi/OdenmeyenMasraflar
        [HttpGet("OdenmeyenMasraflar")]
        public async Task<ActionResult<object>> GetOdenmeyenMasraflar()
        {
            var masraflar = await _context.MasrafTalepleri
                .Include(m => m.Personel)
                .Where(m => m.OnayDurumu == "Onaylandi" && m.OdemeTarihi == null)
                .Select(m => new
                {
                    m.Id,
                    PersonelAdi = m.Personel.AdSoyad,
                    m.MasrafTipi,
                    m.Tutar,
                    ToplamTutar = m.Tutar + (m.KdvTutari ?? 0),
                    m.TalepTarihi
                })
                .ToListAsync();

            var toplamTutar = masraflar.Sum(m => m.ToplamTutar);

            return Ok(new
            {
                Masraflar = masraflar,
                ToplamTutar = toplamTutar,
                MasrafSayisi = masraflar.Count
            });
        }

        // GET: api/MasrafTalebi/AylikRapor
        [HttpGet("AylikRapor")]
        public async Task<ActionResult<object>> GetAylikMasrafRaporu([FromQuery] int yil, [FromQuery] int ay)
        {
            var ayBaslangic = new DateTime(yil, ay, 1);
            var aySonu = ayBaslangic.AddMonths(1);

            var masraflar = await _context.MasrafTalepleri
                .Where(m => m.Tarih >= ayBaslangic && m.Tarih < aySonu && m.OnayDurumu == "Odendi")
                .GroupBy(m => m.MasrafTipi)
                .Select(g => new
                {
                    MasrafTipi = g.Key,
                    ToplamTutar = g.Sum(m => m.Tutar + (m.KdvTutari ?? 0)),
                    MasrafSayisi = g.Count()
                })
                .ToListAsync();

            var genelToplam = masraflar.Sum(m => m.ToplamTutar);

            return Ok(new
            {
                Yil = yil,
                Ay = ay,
                MasrafDetaylari = masraflar,
                GenelToplam = genelToplam
            });
        }

        // Helper Methods
        private async Task SendMasrafBildirimi(int personelId, string mesaj, int masrafId)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == personelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = "Masraf Bildirimi",
                    Mesaj = mesaj,
                    Tip = "Başarı",
                    ReferansTip = "Masraf",
                    ReferansId = masrafId
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTO
    public class MasrafTalebiDTO
    {
        public int PersonelId { get; set; }
        public string MasrafTipi { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public string? Faturalar { get; set; }
        public decimal? KdvOrani { get; set; }
        public List<int> OnaylayiciIds { get; set; }
        public int SirketId { get; set; }
    }
}