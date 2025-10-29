using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
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
    public class GorevController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public GorevController(PDKSDbContext context)
        {
            _context = context;
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


        // GET: api/Gorev
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetGorevler([FromQuery] int? sirketId)
        {
            var query = _context.Gorevler
                .Include(g => g.Atayan)
                .Include(g => g.GorevAtamalari)
                .ThenInclude(ga => ga.Personel)
                .AsQueryable();

            if (sirketId.HasValue)
                query = query.Where(g => g.SirketId == sirketId.Value);

            var gorevler = await query
                .Select(g => new
                {
                    g.Id,
                    g.Baslik,
                    g.Aciklama,
                    AtayanAdi = g.Atayan.AdSoyad,
                    AtananPersoneller = g.GorevAtamalari.Select(ga => ga.Personel.AdSoyad).ToList(),
                    g.BaslangicTarihi,
                    g.BitisTarihi,
                    g.Oncelik,
                    g.Durum,
                    g.TamamlanmaYuzdesi,
                    g.OlusturmaTarihi,
                    AltGorevSayisi = g.AltGorevler.Count
                })
                .OrderByDescending(g => g.OlusturmaTarihi)
                .ToListAsync();

            return Ok(gorevler);
        }

        // GET: api/Gorev/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetGorev(int id)
        {
            var gorev = await _context.Gorevler
                .Include(g => g.Atayan)
                .Include(g => g.GorevAtamalari)
                .ThenInclude(ga => ga.Personel)
                .Include(g => g.AltGorevler)
                .Include(g => g.Yorumlar)
                .ThenInclude(y => y.Personel)
                .Where(g => g.Id == id)
                .Select(g => new
                {
                    g.Id,
                    g.Baslik,
                    g.Aciklama,
                    AtayanAdi = g.Atayan.AdSoyad,
                    g.AtayanPersonelId,
                    AtananPersoneller = g.GorevAtamalari.Select(ga => new
                    {
                        ga.PersonelId,
                        PersonelAdi = ga.Personel.AdSoyad,
                        ga.AtamaTarihi,
                        ga.Tamamlandi,
                        ga.TamamlanmaTarihi
                    }).ToList(),
                    g.BaslangicTarihi,
                    g.BitisTarihi,
                    g.Oncelik,
                    g.Durum,
                    g.TamamlanmaYuzdesi,
                    g.Etiketler,
                    g.Dosyalar,
                    g.UstGorevId,
                    g.OlusturmaTarihi,
                    AltGorevler = g.AltGorevler.Select(ag => new
                    {
                        ag.Id,
                        ag.Baslik,
                        ag.Durum,
                        ag.TamamlanmaYuzdesi
                    }).ToList(),
                    Yorumlar = g.Yorumlar.OrderByDescending(y => y.YorumTarihi).Select(y => new
                    {
                        y.Id,
                        PersonelAdi = y.Personel.AdSoyad,
                        y.Yorum,
                        y.Dosyalar,
                        y.YorumTarihi
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (gorev == null)
                return NotFound();

            return Ok(gorev);
        }

        // POST: api/Gorev
        [HttpPost]
        public async Task<ActionResult<Gorev>> PostGorev([FromBody] GorevDTO dto)
        {
            var gorev = new Gorev
            {
                Baslik = dto.Baslik,
                Aciklama = dto.Aciklama,
                AtayanPersonelId = dto.AtayanPersonelId,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                Oncelik = dto.Oncelik,
                Durum = dto.Durum ?? "Yeni",
                TamamlanmaYuzdesi = 0,
                Etiketler = dto.Etiketler,
                Dosyalar = dto.Dosyalar,
                UstGorevId = dto.UstGorevId,
                SirketId = dto.SirketId
            };

            _context.Gorevler.Add(gorev);
            await _context.SaveChangesAsync();

            // Personel atamaları
            if (dto.AtananPersonelIds != null && dto.AtananPersonelIds.Any())
            {
                foreach (var personelId in dto.AtananPersonelIds)
                {
                    var atama = new GorevAtama
                    {
                        GorevId = gorev.Id,
                        PersonelId = personelId
                    };
                    _context.GorevAtamalari.Add(atama);

                    // Bildirim gönder
                    await SendGorevBildirimi(personelId, gorev.Id, gorev.Baslik);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetGorev), new { id = gorev.Id }, gorev);
        }

        // PUT: api/Gorev/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGorev(int id, [FromBody] GorevDTO dto)
        {
            var gorev = await _context.Gorevler.FindAsync(id);
            if (gorev == null)
                return NotFound();

            gorev.Baslik = dto.Baslik;
            gorev.Aciklama = dto.Aciklama;
            gorev.BaslangicTarihi = dto.BaslangicTarihi;
            gorev.BitisTarihi = dto.BitisTarihi;
            gorev.Oncelik = dto.Oncelik;
            gorev.Durum = dto.Durum ?? gorev.Durum;
            gorev.TamamlanmaYuzdesi = dto.TamamlanmaYuzdesi;
            gorev.Etiketler = dto.Etiketler;
            gorev.Dosyalar = dto.Dosyalar;
            gorev.GuncellemeTarihi = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Gorev/{id}/Yorum
        [HttpPost("{id}/Yorum")]
        public async Task<ActionResult> PostYorum(int id, [FromBody] GorevYorumDTO dto)
        {
            var gorev = await _context.Gorevler.FindAsync(id);
            if (gorev == null)
                return NotFound();

            var yorum = new GorevYorum
            {
                GorevId = id,
                PersonelId = dto.PersonelId,
                Yorum = dto.Yorum,
                Dosyalar = dto.Dosyalar
            };

            _context.GorevYorumlari.Add(yorum);
            await _context.SaveChangesAsync();

            return Ok(yorum);
        }

        // GET: api/Gorev/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelGorevleri(int personelId)
        {
            var gorevler = await _context.GorevAtamalari
                .Include(ga => ga.Gorev)
                .ThenInclude(g => g.Atayan)
                .Where(ga => ga.PersonelId == personelId)
                .Select(ga => new
                {
                    ga.GorevId,
                    ga.Gorev.Baslik,
                    ga.Gorev.Aciklama,
                    AtayanAdi = ga.Gorev.Atayan.AdSoyad,
                    ga.Gorev.BaslangicTarihi,
                    ga.Gorev.BitisTarihi,
                    ga.Gorev.Oncelik,
                    ga.Gorev.Durum,
                    ga.Gorev.TamamlanmaYuzdesi,
                    ga.AtamaTarihi,
                    ga.Tamamlandi
                })
                .OrderByDescending(g => g.AtamaTarihi)
                .ToListAsync();

            return Ok(gorevler);
        }

        // Helper Methods
        private async Task SendGorevBildirimi(int personelId, int gorevId, string gorevBaslik)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == personelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = "Yeni Görev Atandı",
                    Mesaj = $"Size yeni bir görev atandı: {gorevBaslik}",
                    Tip = "Bilgi",
                    ReferansTip = "Gorev",
                    ReferansId = gorevId
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs
    public class GorevDTO
    {
        public string Baslik { get; set; }
        public string? Aciklama { get; set; }
        public int AtayanPersonelId { get; set; }
        public List<int>? AtananPersonelIds { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string Oncelik { get; set; } = "Orta";
        public string? Durum { get; set; }
        public int TamamlanmaYuzdesi { get; set; }
        public string? Etiketler { get; set; }
        public string? Dosyalar { get; set; }
        public int? UstGorevId { get; set; }
        public int SirketId { get; set; }
    }

    public class GorevYorumDTO
    {
        public int PersonelId { get; set; }
        public string Yorum { get; set; }
        public string? Dosyalar { get; set; }
    }
}