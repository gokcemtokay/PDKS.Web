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
    public class AracTalebiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public AracTalebiController(PDKSDbContext context)
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


        // GET: api/AracTalebi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAracTalepleri()
        {
            var talepler = await _context.AracTalepleri
                .Include(a => a.Personel)
                .Include(a => a.Arac)
                .Select(a => new
                {
                    a.Id,
                    PersonelAdi = a.Personel.AdSoyad,
                    a.PersonelId,
                    AracPlaka = a.Arac != null ? a.Arac.Plaka : "Atanmadı",
                    a.GidisSehri,
                    a.DonusSehri,
                    a.KalkisTarihi,
                    a.KalkisSaati,
                    a.DonusTarihi,
                    a.DonusSaati,
                    a.YolcuSayisi,
                    a.OnayDurumu,
                    a.TalepTarihi
                })
                .OrderByDescending(a => a.TalepTarihi)
                .ToListAsync();

            return Ok(talepler);
        }

        // GET: api/AracTalebi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<object>> GetAracTalebi(int id)
        //{
            //var talep = await _context.AracTalepleri
            //    .Include(a => a.Personel)
            //    .Include(a => a.Arac)
            //    .Where(a => a.Id == id)
            //    .Select(a => new
            //    {
            //        a.Id,
            //        PersonelAdi = a.Personel.AdSoyad,
            //        a.PersonelId,
            //        a.AracId,
            //        AracBilgisi = a.Arac != null ? new
            //        {
            //            a.Arac.Plaka,
            //            a.Arac.Marka,
            //            a.Arac.Model,
            //            a.Arac.Renk
            //        } : null,
            //        a.GidisSehri,
            //        a.DonusSehri,
            //        a.KalkisTarihi,
            //        a.KalkisSaati,
            //        a.DonusTarihi,
            //        a.DonusSaati,
            //        a.YolcuSayisi,
            //        a.Amac,
            //        a.OnayDurumu,
            //        a.TalepTarihi,
            //        OnayAkisi = _context.OnayAkislari
            //            .Where(o => o.OnayTipi == "Arac" && o.ReferansId == a.Id)
            //            .Select(o => new
            //            {
            //                o.Sira,
            //                OnaylayiciAdi = o.Onaylayici.AdSoyad,
            //                o.OnayDurumu,
            //                o.OnayTarihi
            //            })
            //            .ToList()
            //    })
            //    .FirstOrDefaultAsync();

            //if (talep == null)
            //    return NotFound();

            //return Ok(talep);
        //}

        // POST: api/AracTalebi
        //[HttpPost]
        //public async Task<ActionResult<AracTalebi>> PostAracTalebi([FromBody] AracTalebiDTO dto)
        //{
        //    // Tarih çakışması kontrolü - aynı araç aynı tarihlerde başka bir talep var mı?
        //    if (dto.AracId.HasValue)
        //    {
        //        var cakismaVarMi = await _context.AracTalepleri
        //            .AnyAsync(a => a.AracId == dto.AracId &&
        //                           a.OnayDurumu == "Onaylandi" &&
        //                           ((a.KalkisTarihi >= dto.KalkisTarihi && a.KalkisTarihi <= dto.DonusTarihi) ||
        //                            (a.DonusTarihi >= dto.KalkisTarihi && a.DonusTarihi <= dto.DonusTarihi)));

        //        if (cakismaVarMi)
        //            return BadRequest("Bu araç seçilen tarihlerde başka bir talep için kullanılıyor.");
        //    }

        //    var aracTalebi = new AracTalebi
        //    {
        //        PersonelId = dto.PersonelId,
        //        AracId = dto.AracId,
        //        GidisSehri = dto.GidisSehri,
        //        DonusSehri = dto.DonusSehri,
        //        KalkisTarihi = dto.KalkisTarihi,
        //        KalkisSaati = dto.KalkisSaati,
        //        DonusTarihi = dto.DonusTarihi,
        //        DonusSaati = dto.DonusSaati,
        //        YolcuSayisi = dto.YolcuSayisi,
        //        Amac = dto.Amac,
        //        SirketId = dto.SirketId
        //    };

        //    _context.AracTalepleri.Add(aracTalebi);
        //    await _context.SaveChangesAsync();

            // Onay akışı oluştur
            //if (dto.OnaylayiciIds != null && dto.OnaylayiciIds.Any())
            //{
            //    for (int i = 0; i < dto.OnaylayiciIds.Count; i++)
            //    {
            //        var onayAkisi = new OnayAkisi
            //        {
            //            OnayTipi = "Arac",
            //            ReferansId = aracTalebi.Id,
            //            Sira = i + 1,
            //            OnaylayiciPersonelId = dto.OnaylayiciIds[i],
            //            SirketId = dto.SirketId
            //        };
            //        _context.OnayAkislari.Add(onayAkisi);
            //    }
            //    await _context.SaveChangesAsync();
            //}

        //    return CreatedAtAction(nameof(GetAracTalebi), new { id = aracTalebi.Id }, aracTalebi);
        //}

        // GET: api/AracTalebi/MusaitAraclar
        [HttpGet("MusaitAraclar")]
        public async Task<ActionResult<IEnumerable<object>>> GetMusaitAraclar([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            // Belirtilen tarihler arasında müsait olan araçları getir
            var mesgulAracIds = await _context.AracTalepleri
                .Where(a => a.OnayDurumu == "Onaylandi" &&
                           ((a.KalkisTarihi >= baslangic && a.KalkisTarihi <= bitis) ||
                            (a.DonusTarihi >= baslangic && a.DonusTarihi <= bitis)))
                .Select(a => a.AracId)
                .ToListAsync();

            var musaitAraclar = await _context.Araclar
                .Where(a => a.Aktif && !mesgulAracIds.Contains(a.Id))
                .Select(a => new
                {
                    a.Id,
                    a.Plaka,
                    a.Marka,
                    a.Model,
                    a.Renk,
                    a.YakitTipi,
                    TahsisliPersonel = a.TahsisliPersonel != null ? a.TahsisliPersonel.AdSoyad : null
                })
                .ToListAsync();

            return Ok(musaitAraclar);
        }

        // PUT: api/AracTalebi/AracAta/{id}
        [HttpPut("AracAta/{id}")]
        public async Task<IActionResult> AracAta(int id, [FromBody] AracAtaDTO dto)
        {
            var talep = await _context.AracTalepleri.FindAsync(id);
            if (talep == null)
                return NotFound();

            talep.AracId = dto.AracId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/AracTalebi/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelAracTalepleri(int personelId)
        {
            var talepler = await _context.AracTalepleri
                .Include(a => a.Arac)
                .Where(a => a.PersonelId == personelId)
                .OrderByDescending(a => a.TalepTarihi)
                .Select(a => new
                {
                    a.Id,
                    AracPlaka = a.Arac != null ? a.Arac.Plaka : "Atanmadı",
                    a.GidisSehri,
                    a.DonusSehri,
                    a.KalkisTarihi,
                    a.DonusTarihi,
                    a.OnayDurumu
                })
                .ToListAsync();

            return Ok(talepler);
        }
    }

    // DTOs
    public class AracTalebiDTO
    {
        public int PersonelId { get; set; }
        public int? AracId { get; set; }
        public string? GidisSehri { get; set; }
        public string? DonusSehri { get; set; }
        public DateTime KalkisTarihi { get; set; }
        public TimeSpan KalkisSaati { get; set; }
        public DateTime DonusTarihi { get; set; }
        public TimeSpan DonusSaati { get; set; }
        public int YolcuSayisi { get; set; }
        public string? Amac { get; set; }
        public List<int> OnaylayiciIds { get; set; }
        public int SirketId { get; set; }
    }

    public class AracAtaDTO
    {
        public int AracId { get; set; }
    }
}