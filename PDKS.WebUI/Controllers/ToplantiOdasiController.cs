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
    public class ToplantiOdasiController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public ToplantiOdasiController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/ToplantiOdasi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetToplantiOdalari()
        {
            var odalar = await _context.ToplantiOdalari
                .Where(o => o.Aktif)
                .Select(o => new
                {
                    o.Id,
                    o.OdaAdi,
                    o.Kapasite,
                    o.Ozellikler,
                    o.Aktif
                })
                .ToListAsync();

            return Ok(odalar);
        }

        // GET: api/ToplantiOdasi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetToplantiOdasi(int id)
        {
            var oda = await _context.ToplantiOdalari
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.OdaAdi,
                    o.Kapasite,
                    o.Ozellikler,
                    o.Aktif
                })
                .FirstOrDefaultAsync();

            if (oda == null)
                return NotFound();

            return Ok(oda);
        }

        // POST: api/ToplantiOdasi
        [HttpPost]
        public async Task<ActionResult<ToplantiOdasi>> PostToplantiOdasi([FromBody] ToplantiOdasiDTO dto)
        {
            var oda = new ToplantiOdasi
            {
                OdaAdi = dto.OdaAdi,
                Kapasite = dto.Kapasite,
                Ozellikler = dto.Ozellikler,
                SirketId = dto.SirketId
            };

            _context.ToplantiOdalari.Add(oda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToplantiOdasi), new { id = oda.Id }, oda);
        }

        // PUT: api/ToplantiOdasi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToplantiOdasi(int id, [FromBody] ToplantiOdasiDTO dto)
        {
            var oda = await _context.ToplantiOdalari.FindAsync(id);
            if (oda == null)
                return NotFound();

            oda.OdaAdi = dto.OdaAdi;
            oda.Kapasite = dto.Kapasite;
            oda.Ozellikler = dto.Ozellikler;
            oda.Aktif = dto.Aktif;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToplantiOdasi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToplantiOdasi(int id)
        {
            var oda = await _context.ToplantiOdalari.FindAsync(id);
            if (oda == null)
                return NotFound();

            oda.Aktif = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ToplantiOdasi/MusaitOdalar
        [HttpGet("MusaitOdalar")]
        public async Task<ActionResult<IEnumerable<object>>> GetMusaitOdalar([FromQuery] DateTime baslangic, [FromQuery] DateTime bitis)
        {
            // Belirtilen tarihler arasında dolu olan odaları bul
            var doluOdaIds = await _context.ToplantiOdasiRezervasyonlari
                .Where(r => r.Durum == "Aktif" &&
                           ((r.BaslangicTarihi >= baslangic && r.BaslangicTarihi < bitis) ||
                            (r.BitisTarihi > baslangic && r.BitisTarihi <= bitis) ||
                            (r.BaslangicTarihi <= baslangic && r.BitisTarihi >= bitis)))
                .Select(r => r.OdaId)
                .Distinct()
                .ToListAsync();

            // Müsait odaları getir
            var musaitOdalar = await _context.ToplantiOdalari
                .Where(o => o.Aktif && !doluOdaIds.Contains(o.Id))
                .Select(o => new
                {
                    o.Id,
                    o.OdaAdi,
                    o.Kapasite,
                    o.Ozellikler
                })
                .ToListAsync();

            return Ok(musaitOdalar);
        }

        // GET: api/ToplantiOdasi/{id}/Rezervasyonlar
        [HttpGet("{id}/Rezervasyonlar")]
        public async Task<ActionResult<IEnumerable<object>>> GetOdaRezervasyonlari(int id, [FromQuery] DateTime? baslangic, [FromQuery] DateTime? bitis)
        {
            var query = _context.ToplantiOdasiRezervasyonlari
                .Include(r => r.Personel)
                .Where(r => r.OdaId == id && r.Durum == "Aktif");

            if (baslangic.HasValue)
                query = query.Where(r => r.BitisTarihi >= baslangic.Value);

            if (bitis.HasValue)
                query = query.Where(r => r.BaslangicTarihi <= bitis.Value);

            var rezervasyonlar = await query
                .Select(r => new
                {
                    r.Id,
                    PersonelAdi = r.Personel.AdSoyad,
                    r.BaslangicTarihi,
                    r.BitisTarihi,
                    r.Konu,
                    r.Katilimcilar
                })
                .OrderBy(r => r.BaslangicTarihi)
                .ToListAsync();

            return Ok(rezervasyonlar);
        }
    }

    // DTO
    public class ToplantiOdasiDTO
    {
        public string OdaAdi { get; set; }
        public int? Kapasite { get; set; }
        public string? Ozellikler { get; set; }
        public bool Aktif { get; set; } = true;
        public int SirketId { get; set; }
    }
}