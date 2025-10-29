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
    public class AvansTalebiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public AvansTalebiController(PDKSDbContext context)
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


        // GET: api/AvansTalebi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAvanslar()
        {
            var avanslar = await _context.AvansTalepleri
                .Include(a => a.Personel)
                .Select(a => new
                {
                    a.Id,
                    PersonelAdi = a.Personel.AdSoyad, // ✅ Düzeltildi
                    a.PersonelId,
                    a.Tutar,
                    a.Sebep,
                    a.TalepTarihi,
                    a.OdemeSekli,
                    a.OnayDurumu,
                    a.OdemeTarihi
                })
                .OrderByDescending(a => a.TalepTarihi)
                .ToListAsync();

            return Ok(avanslar);
        }

        // GET: api/AvansTalebi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<object>> GetAvans(int id)
        //{
        //    var avans = await _context.AvansTalepleri
        //        .Include(a => a.Personel)
        //        .Where(a => a.Id == id)
        //        .Select(a => new
        //        {
        //            a.Id,
        //            PersonelAdi = a.Personel.AdSoyad, // ✅ Düzeltildi
        //            a.PersonelId,
        //            a.Tutar,
        //            a.Sebep,
        //            a.TalepTarihi,
        //            a.OdemeSekli,
        //            a.TaksitSayisi,
        //            a.OnayDurumu,
        //            a.OdemeTarihi,
        //            a.DekontDosyasi,
        //            OnayAkisi = _context.OnayAkislari
        //                .Where(o => o.OnayTipi == "Avans" && o.ReferansId == a.Id)
        //                .Select(o => new
        //                {
        //                    o.Sira,
        //                    OnaylayiciAdi = o.Onaylayici.AdSoyad, // ✅ Düzeltildi
        //                    o.OnayDurumu,
        //                    o.OnayTarihi
        //                })
        //                .ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (avans == null)
        //        return NotFound();

        //    return Ok(avans);
        //}

        // POST: api/AvansTalebi
        //[HttpPost]
        //public async Task<ActionResult<AvansTalebi>> PostAvans([FromBody] AvansTalebiDTO dto)
        //{
        //    var avansTalebi = new AvansTalebi
        //    {
        //        PersonelId = dto.PersonelId,
        //        Tutar = dto.Tutar,
        //        Sebep = dto.Sebep,
        //        OdemeSekli = dto.OdemeSekli,
        //        TaksitSayisi = dto.TaksitSayisi,
        //        SirketId = dto.SirketId
        //    };

        //    _context.AvansTalepleri.Add(avansTalebi);
        //    await _context.SaveChangesAsync();

            // Onay akışı oluştur
            //if (dto.OnaylayiciIds != null && dto.OnaylayiciIds.Any())
            //{
            //    for (int i = 0; i < dto.OnaylayiciIds.Count; i++)
            //    {
            //        var onayAkisi = new OnayAkisi
            //        {
            //            OnayTipi = "Avans",
            //            ReferansId = avansTalebi.Id,
            //            Sira = i + 1,
            //            OnaylayiciPersonelId = dto.OnaylayiciIds[i],
            //            SirketId = dto.SirketId
            //        };
            //        _context.OnayAkislari.Add(onayAkisi);
            //    }
            //    await _context.SaveChangesAsync();
            //}

        //    return CreatedAtAction(nameof(GetAvans), new { id = avansTalebi.Id }, avansTalebi);
        //}

        // GET: api/AvansTalebi/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelAvanslar(int personelId)
        {
            var avanslar = await _context.AvansTalepleri
                .Where(a => a.PersonelId == personelId)
                .OrderByDescending(a => a.TalepTarihi)
                .Select(a => new
                {
                    a.Id,
                    a.Tutar,
                    a.Sebep,
                    a.TalepTarihi,
                    a.OnayDurumu,
                    a.OdemeTarihi
                })
                .ToListAsync();

            return Ok(avanslar);
        }
    }

    // DTO
    public class AvansTalebiDTO
    {
        public int PersonelId { get; set; }
        public decimal Tutar { get; set; }
        public string Sebep { get; set; }
        public string? OdemeSekli { get; set; }
        public int? TaksitSayisi { get; set; }
        public List<int> OnaylayiciIds { get; set; }
        public int SirketId { get; set; }
    }
}