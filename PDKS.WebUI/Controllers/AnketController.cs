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
    public class AnketController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public AnketController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Anket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAnketler()
        {
            var anketler = await _context.Anketler
                .Include(a => a.OlusturanKullanici)
                .ThenInclude(k => k.Personel)
                .Include(a => a.Sorular)
                .Select(a => new
                {
                    a.Id,
                    a.AnketBasligi,
                    a.Aciklama,
                    a.BaslangicTarihi,
                    a.BitisTarihi,
                    a.Anonim,
                    a.Aktif,
                    OlusturanKullanici = a.OlusturanKullanici.Personel.AdSoyad,
                    SoruSayisi = a.Sorular.Count,
                    CevapSayisi = a.Sorular.SelectMany(s => s.Cevaplar).Count(),
                    a.OlusturmaTarihi
                })
                .OrderByDescending(a => a.OlusturmaTarihi)
                .ToListAsync();

            return Ok(anketler);
        }

        // GET: api/Anket/Aktif
        [HttpGet("Aktif")]
        public async Task<ActionResult<IEnumerable<object>>> GetAktifAnketler()
        {
            var bugun = DateTime.UtcNow.Date;

            var anketler = await _context.Anketler
                .Where(a => a.Aktif &&
                           a.BaslangicTarihi <= bugun &&
                           a.BitisTarihi >= bugun)
                .Select(a => new
                {
                    a.Id,
                    a.AnketBasligi,
                    a.Aciklama,
                    a.BitisTarihi,
                    a.Anonim
                })
                .ToListAsync();

            return Ok(anketler);
        }

        // GET: api/Anket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAnket(int id)
        {
            var anket = await _context.Anketler
                .Include(a => a.OlusturanKullanici)
                .ThenInclude(k => k.Personel)
                .Include(a => a.Sorular)
                .ThenInclude(s => s.Cevaplar)
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.AnketBasligi,
                    a.Aciklama,
                    a.BaslangicTarihi,
                    a.BitisTarihi,
                    a.Anonim,
                    a.Aktif,
                    a.HedefKatilimcilar,
                    OlusturanKullanici = a.OlusturanKullanici.Personel.AdSoyad,
                    a.OlusturmaTarihi,
                    Sorular = a.Sorular.OrderBy(s => s.Sira).Select(s => new
                    {
                        s.Id,
                        s.SoruMetni,
                        s.SoruTipi,
                        s.Secenekler,
                        s.Sira,
                        s.Zorunlu,
                        CevapSayisi = s.Cevaplar.Count
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (anket == null)
                return NotFound();

            return Ok(anket);
        }

        // POST: api/Anket
        [HttpPost]
        public async Task<ActionResult<Anket>> PostAnket([FromBody] AnketDTO dto)
        {
            var anket = new Anket
            {
                AnketBasligi = dto.AnketBasligi,
                Aciklama = dto.Aciklama,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                Anonim = dto.Anonim,
                HedefKatilimcilar = dto.HedefKatilimcilar,
                OlusturanKullaniciId = dto.OlusturanKullaniciId,
                SirketId = dto.SirketId
            };

            _context.Anketler.Add(anket);
            await _context.SaveChangesAsync();

            // Soruları ekle
            if (dto.Sorular != null && dto.Sorular.Any())
            {
                foreach (var soruDto in dto.Sorular)
                {
                    var soru = new AnketSoru
                    {
                        AnketId = anket.Id,
                        SoruMetni = soruDto.SoruMetni,
                        SoruTipi = soruDto.SoruTipi,
                        Secenekler = soruDto.Secenekler,
                        Sira = soruDto.Sira,
                        Zorunlu = soruDto.Zorunlu
                    };

                    _context.AnketSorulari.Add(soru);
                }
                await _context.SaveChangesAsync();
            }

            // Hedef katılımcılara bildirim gönder
            await SendAnketBildirimleri(anket);

            return CreatedAtAction(nameof(GetAnket), new { id = anket.Id }, anket);
        }

        // POST: api/Anket/{id}/Cevapla
        [HttpPost("{id}/Cevapla")]
        public async Task<ActionResult> AnketiCevapla(int id, [FromBody] AnketCevapDTO dto)
        {
            var anket = await _context.Anketler
                .Include(a => a.Sorular)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (anket == null)
                return NotFound();

            // Anket aktif mi ve süre içinde mi kontrol et
            var bugun = DateTime.UtcNow.Date;
            if (!anket.Aktif || anket.BaslangicTarihi > bugun || anket.BitisTarihi < bugun)
                return BadRequest("Bu anket şu anda aktif değil.");

            // Daha önce cevaplandı mı kontrol et (anonim değilse)
            if (!anket.Anonim && dto.PersonelId.HasValue)
            {
                var doluMu = await _context.AnketCevaplari
                    .AnyAsync(c => anket.Sorular.Select(s => s.Id).Contains(c.SoruId) &&
                                  c.PersonelId == dto.PersonelId);

                if (doluMu)
                    return BadRequest("Bu anketi daha önce cevapladınız.");
            }

            // Cevapları kaydet
            foreach (var cevapDto in dto.Cevaplar)
            {
                var cevap = new AnketCevap
                {
                    SoruId = cevapDto.SoruId,
                    PersonelId = anket.Anonim ? null : dto.PersonelId,
                    Cevap = cevapDto.Cevap
                };

                _context.AnketCevaplari.Add(cevap);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Anket cevaplarınız kaydedildi. Teşekkürler!" });
        }

        // GET: api/Anket/{id}/Sonuclar
        [HttpGet("{id}/Sonuclar")]
        public async Task<ActionResult<object>> GetAnketSonuclari(int id)
        {
            var anket = await _context.Anketler
                .Include(a => a.Sorular)
                .ThenInclude(s => s.Cevaplar)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (anket == null)
                return NotFound();

            var sonuclar = new List<object>();

            foreach (var soru in anket.Sorular)
            {
                if (soru.SoruTipi == "AcikUclu")
                {
                    // Açık uçlu sorular için
                    sonuclar.Add(new
                    {
                        SoruId = soru.Id,
                        SoruMetni = soru.SoruMetni,
                        SoruTipi = soru.SoruTipi,
                        ToplamCevap = soru.Cevaplar.Count,
                        Cevaplar = soru.Cevaplar.Select(c => c.Cevap).ToList()
                    });
                }
                else
                {
                    // Çoktan seçmeli sorular için
                    var grupluCevaplar = soru.Cevaplar
                        .GroupBy(c => c.Cevap)
                        .Select(g => new
                        {
                            Cevap = g.Key,
                            Sayi = g.Count(),
                            Yuzde = soru.Cevaplar.Count > 0 ? Math.Round(g.Count() * 100.0 / soru.Cevaplar.Count, 2) : 0
                        })
                        .ToList();

                    sonuclar.Add(new
                    {
                        SoruId = soru.Id,
                        SoruMetni = soru.SoruMetni,
                        SoruTipi = soru.SoruTipi,
                        ToplamCevap = soru.Cevaplar.Count,
                        Cevaplar = grupluCevaplar
                    });
                }
            }

            var toplamKatilimci = anket.Sorular
                .SelectMany(s => s.Cevaplar)
                .Select(c => c.PersonelId)
                .Where(p => p.HasValue)
                .Distinct()
                .Count();

            return Ok(new
            {
                AnketBasligi = anket.AnketBasligi,
                ToplamKatilimci = toplamKatilimci,
                Sorular = sonuclar
            });
        }

        // PUT: api/Anket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnket(int id, [FromBody] AnketDTO dto)
        {
            var anket = await _context.Anketler.FindAsync(id);
            if (anket == null)
                return NotFound();

            anket.AnketBasligi = dto.AnketBasligi;
            anket.Aciklama = dto.Aciklama;
            anket.BaslangicTarihi = dto.BaslangicTarihi;
            anket.BitisTarihi = dto.BitisTarihi;
            anket.Anonim = dto.Anonim;
            anket.Aktif = dto.Aktif;
            anket.HedefKatilimcilar = dto.HedefKatilimcilar;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Anket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnket(int id)
        {
            var anket = await _context.Anketler.FindAsync(id);
            if (anket == null)
                return NotFound();

            anket.Aktif = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper Methods
        private async Task SendAnketBildirimleri(Anket anket)
        {
            var hedefKullaniciIds = await _context.Kullanicilar
                .Where(k => k.Aktif)
                .Select(k => k.Id)
                .ToListAsync();

            foreach (var kullaniciId in hedefKullaniciIds)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullaniciId,
                    Baslik = $"Yeni Anket: {anket.AnketBasligi}",
                    Mesaj = $"Lütfen {anket.BitisTarihi:dd.MM.yyyy} tarihine kadar anketi cevaplayın.",
                    Tip = "Bilgi",
                    ReferansTip = "Anket",
                    ReferansId = anket.Id
                };

                _context.Bildirimler.Add(bildirim);
            }

            await _context.SaveChangesAsync();
        }
    }

    // DTOs
    public class AnketDTO
    {
        public string AnketBasligi { get; set; }
        public string? Aciklama { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool Anonim { get; set; }
        public bool Aktif { get; set; } = true;
        public string? HedefKatilimcilar { get; set; }
        public int OlusturanKullaniciId { get; set; }
        public int SirketId { get; set; }
        public List<AnketSoruDTO>? Sorular { get; set; }
    }

    public class AnketSoruDTO
    {
        public string SoruMetni { get; set; }
        public string SoruTipi { get; set; }
        public string? Secenekler { get; set; }
        public int Sira { get; set; }
        public bool Zorunlu { get; set; }
    }

    public class AnketCevapDTO
    {
        public int? PersonelId { get; set; }
        public List<CevapItemDTO> Cevaplar { get; set; }
    }

    public class CevapItemDTO
    {
        public int SoruId { get; set; }
        public string Cevap { get; set; }
    }
}