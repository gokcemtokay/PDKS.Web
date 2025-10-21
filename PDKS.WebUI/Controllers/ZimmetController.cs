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
    public class ZimmetController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public ZimmetController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Zimmet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetZimmetler()
        {
            var zimmetler = await _context.Zimmetler
                .Include(z => z.Personel)
                .Select(z => new
                {
                    z.Id,
                    PersonelAdi = z.Personel.AdSoyad,
                    z.PersonelId,
                    z.DemirbasAdi,
                    z.DemirbasKodu,
                    z.Aciklama,
                    z.TeslimTarihi,
                    z.IadeTarihi,
                    z.Durum,
                    z.OlusturmaTarihi
                })
                .OrderByDescending(z => z.TeslimTarihi)
                .ToListAsync();

            return Ok(zimmetler);
        }

        // GET: api/Zimmet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetZimmet(int id)
        {
            var zimmet = await _context.Zimmetler
                .Include(z => z.Personel)
                .Where(z => z.Id == id)
                .Select(z => new
                {
                    z.Id,
                    PersonelAdi = z.Personel.AdSoyad,
                    z.PersonelId,
                    z.DemirbasAdi,
                    z.DemirbasKodu,
                    z.Aciklama,
                    z.TeslimTarihi,
                    z.IadeTarihi,
                    z.Durum,
                    z.OlusturmaTarihi
                })
                .FirstOrDefaultAsync();

            if (zimmet == null)
                return NotFound();

            return Ok(zimmet);
        }

        // POST: api/Zimmet
        [HttpPost]
        public async Task<ActionResult<Zimmet>> PostZimmet([FromBody] ZimmetDTO dto)
        {
            // Aynı demirbaş kodu başka bir personelde var mı kontrol et
            var mevcutZimmet = await _context.Zimmetler
                .FirstOrDefaultAsync(z => z.DemirbasKodu == dto.DemirbasKodu && z.Durum == "Personelde");

            if (mevcutZimmet != null)
                return BadRequest("Bu demirbaş başka bir personelde kayıtlı.");

            var zimmet = new Zimmet
            {
                PersonelId = dto.PersonelId,
                DemirbasAdi = dto.DemirbasAdi,
                DemirbasKodu = dto.DemirbasKodu,
                Aciklama = dto.Aciklama,
                TeslimTarihi = dto.TeslimTarihi,
                SirketId = dto.SirketId
            };

            _context.Zimmetler.Add(zimmet);
            await _context.SaveChangesAsync();

            // Personele bildirim gönder
            await SendZimmetBildirimi(dto.PersonelId, zimmet.DemirbasAdi, "teslim edildi");

            return CreatedAtAction(nameof(GetZimmet), new { id = zimmet.Id }, zimmet);
        }

        // PUT: api/Zimmet/IadeAl/{id}
        [HttpPut("IadeAl/{id}")]
        public async Task<IActionResult> IadeAl(int id, [FromBody] IadeDTO dto)
        {
            var zimmet = await _context.Zimmetler.FindAsync(id);
            if (zimmet == null)
                return NotFound();

            zimmet.IadeTarihi = dto.IadeTarihi ?? DateTime.UtcNow;
            zimmet.Durum = "İade Edildi";

            await _context.SaveChangesAsync();

            // Personele bildirim gönder
            await SendZimmetBildirimi(zimmet.PersonelId, zimmet.DemirbasAdi, "iade alındı");

            return NoContent();
        }

        // GET: api/Zimmet/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelZimmetleri(int personelId)
        {
            var zimmetler = await _context.Zimmetler
                .Where(z => z.PersonelId == personelId)
                .OrderByDescending(z => z.TeslimTarihi)
                .Select(z => new
                {
                    z.Id,
                    z.DemirbasAdi,
                    z.DemirbasKodu,
                    z.TeslimTarihi,
                    z.IadeTarihi,
                    z.Durum
                })
                .ToListAsync();

            return Ok(zimmetler);
        }

        // GET: api/Zimmet/AktifZimmetler/{personelId}
        [HttpGet("AktifZimmetler/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAktifZimmetler(int personelId)
        {
            var zimmetler = await _context.Zimmetler
                .Where(z => z.PersonelId == personelId && z.Durum == "Personelde")
                .Select(z => new
                {
                    z.Id,
                    z.DemirbasAdi,
                    z.DemirbasKodu,
                    z.TeslimTarihi,
                    KullanimSuresi = (DateTime.UtcNow - z.TeslimTarihi).Days
                })
                .ToListAsync();

            return Ok(zimmetler);
        }

        // DELETE: api/Zimmet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZimmet(int id)
        {
            var zimmet = await _context.Zimmetler.FindAsync(id);
            if (zimmet == null)
                return NotFound();

            _context.Zimmetler.Remove(zimmet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper Methods
        private async Task SendZimmetBildirimi(int personelId, string demirbasAdi, string islem)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == personelId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = $"Zimmet {islem}",
                    Mesaj = $"{demirbasAdi} adlı demirbaş {islem}.",
                    Tip = "Bilgi"
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs
    public class ZimmetDTO
    {
        public int PersonelId { get; set; }
        public string DemirbasAdi { get; set; }
        public string? DemirbasKodu { get; set; }
        public string? Aciklama { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public int SirketId { get; set; }
    }

    public class IadeDTO
    {
        public DateTime? IadeTarihi { get; set; }
    }
}