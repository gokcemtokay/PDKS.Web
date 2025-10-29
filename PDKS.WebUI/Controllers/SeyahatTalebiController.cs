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
    public class SeyahatTalebiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public SeyahatTalebiController(PDKSDbContext context)
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


        // GET: api/SeyahatTalebi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSeyahatTalepleri()
        {
            var talepler = await _context.SeyahatTalepleri
                .Include(s => s.Personel)
                .Select(s => new
                {
                    s.Id,
                    PersonelAdi = s.Personel.AdSoyad,
                    s.PersonelId,
                    s.SeyahatTipi,
                    s.GidisSehri,
                    s.VarisSehri,
                    s.UlkeAdi,
                    s.KalkisTarihi,
                    s.DonusTarihi,
                    s.OnayDurumu,
                    s.TalepTarihi,
                    s.BeklenenMaliyet
                })
                .OrderByDescending(s => s.TalepTarihi)
                .ToListAsync();

            return Ok(talepler);
        }

        // GET: api/SeyahatTalebi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<object>> GetSeyahatTalebi(int id)
        //{
        //    var talep = await _context.SeyahatTalepleri
        //        .Include(s => s.Personel)
        //        .Include(s => s.Masraflar)
        //        .Where(s => s.Id == id)
        //        .Select(s => new
        //        {
        //            s.Id,
        //            PersonelAdi = s.Personel.AdSoyad,
        //            s.PersonelId,
        //            s.SeyahatTipi,
        //            s.GidisSehri,
        //            s.VarisSehri,
        //            s.UlkeAdi,
        //            s.KalkisTarihi,
        //            s.DonusTarihi,
        //            s.Amac,
        //            s.KonaklamaGerekli,
        //            s.UlasimTipi,
        //            s.BeklenenMaliyet,
        //            s.OnayDurumu,
        //            s.TalepTarihi,
        //            s.UcakBileti,
        //            s.OtelRezervasyon,
        //            Masraflar = s.Masraflar.Select(m => new
        //            {
        //                m.Id,
        //                m.MasrafTipi,
        //                m.Tutar,
        //                m.Tarih,
        //                m.Aciklama,
        //                m.FaturaDosyasi
        //            }).ToList(),
        //            ToplamMasraf = s.Masraflar.Sum(m => m.Tutar),
        //            OnayAkisi = _context.OnayAkislari
        //                .Where(o => o.OnayTipi == "Seyahat" && o.ReferansId == s.Id)
        //                .Select(o => new
        //                {
        //                    o.Sira,
        //                    OnaylayiciAdi = o.Onaylayici.AdSoyad,
        //                    o.OnayDurumu,
        //                    o.OnayTarihi,
        //                    o.Aciklama
        //                })
        //                .ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (talep == null)
        //        return NotFound();

        //    return Ok(talep);
        //}

        // POST: api/SeyahatTalebi
        //[HttpPost]
        //public async Task<ActionResult<SeyahatTalebi>> PostSeyahatTalebi([FromBody] SeyahatTalebiDTO dto)
        //{
        //    var seyahatTalebi = new SeyahatTalebi
        //    {
        //        PersonelId = dto.PersonelId,
        //        SeyahatTipi = dto.SeyahatTipi,
        //        GidisSehri = dto.GidisSehri,
        //        VarisSehri = dto.VarisSehri,
        //        UlkeAdi = dto.UlkeAdi,
        //        KalkisTarihi = dto.KalkisTarihi,
        //        DonusTarihi = dto.DonusTarihi,
        //        Amac = dto.Amac,
        //        KonaklamaGerekli = dto.KonaklamaGerekli,
        //        UlasimTipi = dto.UlasimTipi,
        //        BeklenenMaliyet = dto.BeklenenMaliyet,
        //        SirketId = dto.SirketId
        //    };

        //    _context.SeyahatTalepleri.Add(seyahatTalebi);
        //    await _context.SaveChangesAsync();

            // Onay akışı oluştur
            //if (dto.OnaylayiciIds != null && dto.OnaylayiciIds.Any())
            //{
            //    for (int i = 0; i < dto.OnaylayiciIds.Count; i++)
            //    {
            //        var onayAkisi = new OnayAkisi
            //        {
            //            OnayTipi = "Seyahat",
            //            ReferansId = seyahatTalebi.Id,
            //            Sira = i + 1,
            //            OnaylayiciPersonelId = dto.OnaylayiciIds[i],
            //            SirketId = dto.SirketId
            //        };
            //        _context.OnayAkislari.Add(onayAkisi);
            //    }
            //    await _context.SaveChangesAsync();
            //}

        //    return CreatedAtAction(nameof(GetSeyahatTalebi), new { id = seyahatTalebi.Id }, seyahatTalebi);
        //}

        // POST: api/SeyahatTalebi/{id}/Masraf
        [HttpPost("{id}/Masraf")]
        public async Task<ActionResult<SeyahatMasraf>> PostSeyahatMasraf(int id, [FromBody] SeyahatMasrafDTO dto)
        {
            var seyahat = await _context.SeyahatTalepleri.FindAsync(id);
            if (seyahat == null)
                return NotFound();

            var masraf = new SeyahatMasraf
            {
                SeyahatId = id,
                MasrafTipi = dto.MasrafTipi,
                Tutar = dto.Tutar,
                Tarih = dto.Tarih,
                Aciklama = dto.Aciklama,
                FaturaDosyasi = dto.FaturaDosyasi,
                KdvOrani = dto.KdvOrani
            };

            _context.SeyahatMasraflari.Add(masraf);
            await _context.SaveChangesAsync();

            return Ok(masraf);
        }

        // GET: api/SeyahatTalebi/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelSeyahatTalepleri(int personelId)
        {
            var talepler = await _context.SeyahatTalepleri
                .Where(s => s.PersonelId == personelId)
                .OrderByDescending(s => s.TalepTarihi)
                .Select(s => new
                {
                    s.Id,
                    s.SeyahatTipi,
                    s.GidisSehri,
                    s.VarisSehri,
                    s.KalkisTarihi,
                    s.DonusTarihi,
                    s.OnayDurumu
                })
                .ToListAsync();

            return Ok(talepler);
        }

        // GET: api/SeyahatTalebi/AktifSeyahatler
        [HttpGet("AktifSeyahatler")]
        public async Task<ActionResult<IEnumerable<object>>> GetAktifSeyahatler()
        {
            var bugun = DateTime.UtcNow.Date;

            var aktifSeyahatler = await _context.SeyahatTalepleri
                .Include(s => s.Personel)
                .Where(s => s.OnayDurumu == "Onaylandi" &&
                           s.KalkisTarihi <= bugun &&
                           s.DonusTarihi >= bugun)
                .Select(s => new
                {
                    s.Id,
                    PersonelAdi = s.Personel.AdSoyad,
                    s.GidisSehri,
                    s.VarisSehri,
                    s.KalkisTarihi,
                    s.DonusTarihi,
                    s.Amac
                })
                .ToListAsync();

            return Ok(aktifSeyahatler);
        }

        // GET: api/SeyahatTalebi/AylikRapor
        [HttpGet("AylikRapor")]
        public async Task<ActionResult<object>> GetAylikSeyahatRaporu([FromQuery] int yil, [FromQuery] int ay)
        {
            var ayBaslangic = new DateTime(yil, ay, 1);
            var aySonu = ayBaslangic.AddMonths(1);

            var seyahatler = await _context.SeyahatTalepleri
                .Include(s => s.Masraflar)
                .Where(s => s.KalkisTarihi >= ayBaslangic &&
                           s.KalkisTarihi < aySonu &&
                           s.OnayDurumu == "Onaylandi")
                .ToListAsync();

            var yurticiSayisi = seyahatler.Count(s => s.SeyahatTipi == "Yurtiçi");
            var yurtdisiSayisi = seyahatler.Count(s => s.SeyahatTipi == "Yurtdışı");

            var toplamMasraf = seyahatler.Sum(s => s.Masraflar.Sum(m => m.Tutar));

            return Ok(new
            {
                Yil = yil,
                Ay = ay,
                YurticiSayisi = yurticiSayisi,
                YurtdisiSayisi = yurtdisiSayisi,
                ToplamSeyahat = seyahatler.Count,
                ToplamMasraf = toplamMasraf,
                OrtalamaMasraf = seyahatler.Count > 0 ? toplamMasraf / seyahatler.Count : 0
            });
        }

        // PUT: api/SeyahatTalebi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeyahatTalebi(int id, [FromBody] SeyahatTalebiDTO dto)
        {
            var seyahat = await _context.SeyahatTalepleri.FindAsync(id);
            if (seyahat == null)
                return NotFound();

            seyahat.SeyahatTipi = dto.SeyahatTipi;
            seyahat.GidisSehri = dto.GidisSehri;
            seyahat.VarisSehri = dto.VarisSehri;
            seyahat.UlkeAdi = dto.UlkeAdi;
            seyahat.KalkisTarihi = dto.KalkisTarihi;
            seyahat.DonusTarihi = dto.DonusTarihi;
            seyahat.Amac = dto.Amac;
            seyahat.KonaklamaGerekli = dto.KonaklamaGerekli;
            seyahat.UlasimTipi = dto.UlasimTipi;
            seyahat.BeklenenMaliyet = dto.BeklenenMaliyet;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/SeyahatTalebi/{id}/Masraf/{masrafId}
        [HttpDelete("{id}/Masraf/{masrafId}")]
        public async Task<IActionResult> DeleteSeyahatMasraf(int id, int masrafId)
        {
            var masraf = await _context.SeyahatMasraflari
                .FirstOrDefaultAsync(m => m.Id == masrafId && m.SeyahatId == id);

            if (masraf == null)
                return NotFound();

            _context.SeyahatMasraflari.Remove(masraf);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTOs
    public class SeyahatTalebiDTO
    {
        public int PersonelId { get; set; }
        public string SeyahatTipi { get; set; }
        public string GidisSehri { get; set; }
        public string VarisSehri { get; set; }
        public string? UlkeAdi { get; set; }
        public DateTime KalkisTarihi { get; set; }
        public DateTime DonusTarihi { get; set; }
        public string Amac { get; set; }
        public bool KonaklamaGerekli { get; set; }
        public string? UlasimTipi { get; set; }
        public decimal? BeklenenMaliyet { get; set; }
        public List<int> OnaylayiciIds { get; set; }
        public int SirketId { get; set; }
    }

    public class SeyahatMasrafDTO
    {
        public string MasrafTipi { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }
        public string? FaturaDosyasi { get; set; }
        public decimal? KdvOrani { get; set; }
    }
}