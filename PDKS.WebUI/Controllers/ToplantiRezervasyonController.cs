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
    public class ToplantiRezervasyonController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public ToplantiRezervasyonController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/ToplantiRezervasyon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRezervasyonlar()
        {
            var rezervasyonlar = await _context.ToplantiOdasiRezervasyonlari
                .Include(r => r.Oda)
                .Include(r => r.Personel)
                .Where(r => r.Durum == "Aktif")
                .Select(r => new
                {
                    r.Id,
                    OdaAdi = r.Oda.OdaAdi,
                    PersonelAdi = r.Personel.AdSoyad,
                    r.BaslangicTarihi,
                    r.BitisTarihi,
                    r.Konu,
                    r.Katilimcilar,
                    r.Durum
                })
                .OrderBy(r => r.BaslangicTarihi)
                .ToListAsync();

            return Ok(rezervasyonlar);
        }

        // GET: api/ToplantiRezervasyon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRezervasyon(int id)
        {
            var rezervasyon = await _context.ToplantiOdasiRezervasyonlari
                .Include(r => r.Oda)
                .Include(r => r.Personel)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.OdaId,
                    OdaAdi = r.Oda.OdaAdi,
                    r.PersonelId,
                    PersonelAdi = r.Personel.AdSoyad,
                    r.BaslangicTarihi,
                    r.BitisTarihi,
                    r.Konu,
                    r.Katilimcilar,
                    r.Durum
                })
                .FirstOrDefaultAsync();

            if (rezervasyon == null)
                return NotFound();

            return Ok(rezervasyon);
        }

        // POST: api/ToplantiRezervasyon
        [HttpPost]
        public async Task<ActionResult<ToplantiOdasiRezervasyon>> PostRezervasyon([FromBody] RezervasyonDTO dto)
        {
            // Çakışma kontrolü
            var cakismaVarMi = await _context.ToplantiOdasiRezervasyonlari
                .AnyAsync(r => r.OdaId == dto.OdaId &&
                              r.Durum == "Aktif" &&
                              ((r.BaslangicTarihi >= dto.BaslangicTarihi && r.BaslangicTarihi < dto.BitisTarihi) ||
                               (r.BitisTarihi > dto.BaslangicTarihi && r.BitisTarihi <= dto.BitisTarihi) ||
                               (r.BaslangicTarihi <= dto.BaslangicTarihi && r.BitisTarihi >= dto.BitisTarihi)));

            if (cakismaVarMi)
                return BadRequest("Bu oda seçilen tarihler arasında dolu.");

            var rezervasyon = new ToplantiOdasiRezervasyon
            {
                OdaId = dto.OdaId,
                PersonelId = dto.PersonelId,
                BaslangicTarihi = dto.BaslangicTarihi,
                BitisTarihi = dto.BitisTarihi,
                Konu = dto.Konu,
                Katilimcilar = dto.Katilimcilar,
                SirketId = dto.SirketId
            };

            _context.ToplantiOdasiRezervasyonlari.Add(rezervasyon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRezervasyon), new { id = rezervasyon.Id }, rezervasyon);
        }

        // PUT: api/ToplantiRezervasyon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervasyon(int id, [FromBody] RezervasyonDTO dto)
        {
            var rezervasyon = await _context.ToplantiOdasiRezervasyonlari.FindAsync(id);
            if (rezervasyon == null)
                return NotFound();

            // Çakışma kontrolü (kendisi hariç)
            var cakismaVarMi = await _context.ToplantiOdasiRezervasyonlari
                .AnyAsync(r => r.Id != id &&
                              r.OdaId == dto.OdaId &&
                              r.Durum == "Aktif" &&
                              ((r.BaslangicTarihi >= dto.BaslangicTarihi && r.BaslangicTarihi < dto.BitisTarihi) ||
                               (r.BitisTarihi > dto.BaslangicTarihi && r.BitisTarihi <= dto.BitisTarihi) ||
                               (r.BaslangicTarihi <= dto.BaslangicTarihi && r.BitisTarihi >= dto.BitisTarihi)));

            if (cakismaVarMi)
                return BadRequest("Bu oda seçilen tarihler arasında dolu.");

            rezervasyon.OdaId = dto.OdaId;
            rezervasyon.BaslangicTarihi = dto.BaslangicTarihi;
            rezervasyon.BitisTarihi = dto.BitisTarihi;
            rezervasyon.Konu = dto.Konu;
            rezervasyon.Katilimcilar = dto.Katilimcilar;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ToplantiRezervasyon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervasyon(int id)
        {
            var rezervasyon = await _context.ToplantiOdasiRezervasyonlari.FindAsync(id);
            if (rezervasyon == null)
                return NotFound();

            rezervasyon.Durum = "İptal"; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ToplantiRezervasyon/Personel/{personelId}
        [HttpGet("Personel/{personelId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonelRezervasyonlari(int personelId)
        {
            var rezervasyonlar = await _context.ToplantiOdasiRezervasyonlari
                .Include(r => r.Oda)
                .Where(r => r.PersonelId == personelId && r.Durum == "Aktif")
                .OrderBy(r => r.BaslangicTarihi)
                .Select(r => new
                {
                    r.Id,
                    OdaAdi = r.Oda.OdaAdi,
                    r.BaslangicTarihi,
                    r.BitisTarihi,
                    r.Konu
                })
                .ToListAsync();

            return Ok(rezervasyonlar);
        }

        // GET: api/ToplantiRezervasyon/Gunluk
        [HttpGet("Gunluk")]
        public async Task<ActionResult<IEnumerable<object>>> GetGunlukRezervasyonlar([FromQuery] DateTime tarih)
        {
            var gunBaslangic = tarih.Date;
            var gunBitis = gunBaslangic.AddDays(1);

            var rezervasyonlar = await _context.ToplantiOdasiRezervasyonlari
                .Include(r => r.Oda)
                .Include(r => r.Personel)
                .Where(r => r.Durum == "Aktif" &&
                           r.BaslangicTarihi >= gunBaslangic &&
                           r.BaslangicTarihi < gunBitis)
                .OrderBy(r => r.BaslangicTarihi)
                .Select(r => new
                {
                    r.Id,
                    OdaAdi = r.Oda.OdaAdi,
                    PersonelAdi = r.Personel.AdSoyad,
                    r.BaslangicTarihi,
                    r.BitisTarihi,
                    r.Konu,
                    Katilimcilar = r.Katilimcilar
                })
                .ToListAsync();

            return Ok(rezervasyonlar);
        }
    }

    // DTO
    public class RezervasyonDTO
    {
        public int OdaId { get; set; }
        public int PersonelId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Konu { get; set; }
        public string? Katilimcilar { get; set; }
        public int SirketId { get; set; }
    }
}