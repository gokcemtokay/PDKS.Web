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
    public class AracController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public AracController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Arac
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAraclar()
        {
            var araclar = await _context.Araclar
                .Include(a => a.TahsisliPersonel)
                .Select(a => new
                {
                    a.Id,
                    a.Plaka,
                    a.Marka,
                    a.Model,
                    a.Yil,
                    a.Renk,
                    a.YakitTipi,
                    a.GuncelKm,
                    a.SonMuayeneTarihi,
                    a.KaskoTarihi,
                    a.SigortaTarihi,
                    a.Aktif,
                    TahsisliPersonel = a.TahsisliPersonel != null ? a.TahsisliPersonel.AdSoyad : null
                })
                .ToListAsync();

            return Ok(araclar);
        }

        // GET: api/Arac/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetArac(int id)
        {
            var arac = await _context.Araclar
                .Include(a => a.TahsisliPersonel)
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Plaka,
                    a.Marka,
                    a.Model,
                    a.Yil,
                    a.Renk,
                    a.YakitTipi,
                    a.GuncelKm,
                    a.SonMuayeneTarihi,
                    a.KaskoTarihi,
                    a.SigortaTarihi,
                    a.Aktif,
                    a.TahsisliPersonelId,
                    TahsisliPersonel = a.TahsisliPersonel != null ? a.TahsisliPersonel.AdSoyad : null
                })
                .FirstOrDefaultAsync();

            if (arac == null)
                return NotFound();

            return Ok(arac);
        }

        // POST: api/Arac
        [HttpPost]
        public async Task<ActionResult<Arac>> PostArac([FromBody] AracDTO dto)
        {
            // Plaka kontrolü
            var mevcutArac = await _context.Araclar.FirstOrDefaultAsync(a => a.Plaka == dto.Plaka);
            if (mevcutArac != null)
                return BadRequest("Bu plaka zaten kayıtlı.");

            var arac = new Arac
            {
                Plaka = dto.Plaka,
                Marka = dto.Marka,
                Model = dto.Model,
                Yil = dto.Yil,
                Renk = dto.Renk,
                YakitTipi = dto.YakitTipi,
                GuncelKm = dto.GuncelKm,
                SonMuayeneTarihi = dto.SonMuayeneTarihi,
                KaskoTarihi = dto.KaskoTarihi,
                SigortaTarihi = dto.SigortaTarihi,
                TahsisliPersonelId = dto.TahsisliPersonelId,
                SirketId = dto.SirketId
            };

            _context.Araclar.Add(arac);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArac), new { id = arac.Id }, arac);
        }

        // PUT: api/Arac/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArac(int id, [FromBody] AracDTO dto)
        {
            var arac = await _context.Araclar.FindAsync(id);
            if (arac == null)
                return NotFound();

            arac.Plaka = dto.Plaka;
            arac.Marka = dto.Marka;
            arac.Model = dto.Model;
            arac.Yil = dto.Yil;
            arac.Renk = dto.Renk;
            arac.YakitTipi = dto.YakitTipi;
            arac.GuncelKm = dto.GuncelKm;
            arac.SonMuayeneTarihi = dto.SonMuayeneTarihi;
            arac.KaskoTarihi = dto.KaskoTarihi;
            arac.SigortaTarihi = dto.SigortaTarihi;
            arac.TahsisliPersonelId = dto.TahsisliPersonelId;
            arac.Aktif = dto.Aktif;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Arac/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArac(int id)
        {
            var arac = await _context.Araclar.FindAsync(id);
            if (arac == null)
                return NotFound();

            arac.Aktif = false; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Arac/BakimUyarilari
        [HttpGet("BakimUyarilari")]
        public async Task<ActionResult<object>> GetBakimUyarilari()
        {
            var bugun = DateTime.UtcNow.Date;
            var onBesDun = bugun.AddDays(15);

            var uyarilar = await _context.Araclar
                .Where(a => a.Aktif &&
                           (a.SonMuayeneTarihi <= onBesDun ||
                            a.KaskoTarihi <= onBesDun ||
                            a.SigortaTarihi <= onBesDun))
                .Select(a => new
                {
                    a.Id,
                    a.Plaka,
                    a.Marka,
                    a.Model,
                    Uyarilar = new List<string>
                    {
                        a.SonMuayeneTarihi <= onBesDun ? $"Muayene: {a.SonMuayeneTarihi:dd.MM.yyyy}" : null,
                        a.KaskoTarihi <= onBesDun ? $"Kasko: {a.KaskoTarihi:dd.MM.yyyy}" : null,
                        a.SigortaTarihi <= onBesDun ? $"Sigorta: {a.SigortaTarihi:dd.MM.yyyy}" : null
                    }.Where(u => u != null).ToList()
                })
                .ToListAsync();

            return Ok(uyarilar);
        }
    }

    // DTO
    public class AracDTO
    {
        public string Plaka { get; set; }
        public string? Marka { get; set; }
        public string? Model { get; set; }
        public int? Yil { get; set; }
        public string? Renk { get; set; }
        public string? YakitTipi { get; set; }
        public int GuncelKm { get; set; }
        public DateTime? SonMuayeneTarihi { get; set; }
        public DateTime? KaskoTarihi { get; set; }
        public DateTime? SigortaTarihi { get; set; }
        public int? TahsisliPersonelId { get; set; }
        public bool Aktif { get; set; } = true;
        public int SirketId { get; set; }
    }
}