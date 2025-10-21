using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;
using PDKS.Business.Services;
using PDKS.WebUI.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OnayAkisiController : ControllerBase
    {
        private readonly PDKSDbContext _context;
        private readonly IOnayAkisiService _onayAkisiService;
        private readonly IBildirimService _bildirimService;
        public OnayAkisiController(PDKSDbContext context, IOnayAkisiService onayAkisiService, IBildirimService bildirimService)
        {
            _context = context;
            _onayAkisiService = onayAkisiService;
            _bildirimService = bildirimService;
        }

        // GET: api/OnayAkisi/BekleyenOnaylar
        [HttpGet("BekleyenOnaylar")]
        public async Task<ActionResult<IEnumerable<object>>> GetBekleyenOnaylar()
        {
            var personelId = int.Parse(User.FindFirst("personelId")?.Value ?? "0");

            var onaylarTemp = await _context.OnayAkislari
                .Where(o => o.OnaylayiciPersonelId == personelId && o.OnayDurumu == "Beklemede")
                .OrderBy(o => o.Sira)
                .ToListAsync();

            var onaylar = new List<object>();

            foreach (var onay in onaylarTemp)
            {
                var talepSahibiId = GetTalepSahibiId(onay.OnayTipi, onay.ReferansId);
                var personelAdi = await _context.Personeller
                    .Where(p => p.Id == talepSahibiId)
                    .Select(p => p.AdSoyad)
                    .FirstOrDefaultAsync();

                onaylar.Add(new
                {
                    onay.Id,
                    onay.OnayTipi,
                    onay.ReferansId,
                    onay.Sira,
                    onay.OnayDurumu,
                    onay.OlusturmaTarihi,
                    TalepSahibi = personelAdi ?? "Bilinmiyor"
                });
            }

            return Ok(onaylar);
        }

        // POST: api/OnayAkisi/Onayla
        [HttpPost("Onayla")]
        public async Task<IActionResult> Onayla([FromBody] OnayDTO onayDto)
        {
            var personelId = int.Parse(User.FindFirst("personelId")?.Value ?? "0");

            var sonuc = await _onayAkisiService.OnaylaAsync(
                onayDto.OnayAkisiId,
                onayDto.Onaylandi,
                onayDto.Aciklama,
                personelId
            );

            if (!sonuc)
                return BadRequest(new { message = "Onay işlemi başarısız." });

            // Bildirim gönder
            var onayAkisi = await _context.OnayAkislari.FindAsync(onayDto.OnayAkisiId);
            if (onayAkisi != null)
            {
                await SendBildirim(onayAkisi, onayDto.Onaylandi);
            }

            return Ok(new { message = onayDto.Onaylandi ? "Onaylandı" : "Reddedildi" });
        }

        // GET: api/OnayAkisi/OnayGecmisi/{onayTipi}/{referansId}
        [HttpGet("OnayGecmisi/{onayTipi}/{referansId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOnayGecmisi(string onayTipi, int referansId)
        {
            var gecmis = await _onayAkisiService.GetOnayGecmisiAsync(onayTipi, referansId);

            var result = new List<object>();

            foreach (var onay in gecmis.OrderBy(o => o.Sira))
            {
                var onaylayiciAdi = await _context.Personeller
                    .Where(p => p.Id == onay.OnaylayiciPersonelId)
                    .Select(p => p.AdSoyad)
                    .FirstOrDefaultAsync();

                result.Add(new
                {
                    onay.Id,
                    onay.Sira,
                    OnaylayiciAdi = onaylayiciAdi ?? "Bilinmiyor",
                    onay.OnayDurumu,
                    onay.OnayTarihi,
                    onay.Aciklama
                });
            }

            return Ok(result);
        }

        // Helper Methods
        private int GetTalepSahibiId(string onayTipi, int referansId)
        {
            return onayTipi switch
            {
                "Izin" => _context.Izinler.Where(i => i.Id == referansId).Select(i => i.PersonelId).FirstOrDefault(),
                "Avans" => _context.AvansTalepleri.Where(a => a.Id == referansId).Select(a => a.PersonelId).FirstOrDefault(),
                "Masraf" => _context.MasrafTalepleri.Where(m => m.Id == referansId).Select(m => m.PersonelId).FirstOrDefault(),
                "Arac" => _context.AracTalepleri.Where(a => a.Id == referansId).Select(a => a.PersonelId).FirstOrDefault(),
                "Seyahat" => _context.SeyahatTalepleri.Where(s => s.Id == referansId).Select(s => s.PersonelId).FirstOrDefault(),
                _ => 0
            };
        }

        private async Task SendBildirim(OnayAkisi onayAkisi, bool onaylandi)
        {
            int talepSahibiId = GetTalepSahibiId(onayAkisi.OnayTipi, onayAkisi.ReferansId);

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.PersonelId == talepSahibiId);

            if (kullanici != null)
            {
                var bildirim = new Bildirim
                {
                    KullaniciId = kullanici.Id,
                    Baslik = $"{onayAkisi.OnayTipi} Talebi {(onaylandi ? "Onaylandı" : "Reddedildi")}",
                    Mesaj = $"{onayAkisi.OnayTipi} talebiniz {(onaylandi ? "onaylanmıştır" : "reddedilmiştir")}.",
                    Tip = onaylandi ? "Başarı" : "Uyarı",
                    ReferansTip = onayAkisi.OnayTipi,
                    ReferansId = onayAkisi.ReferansId
                };

                _context.Bildirimler.Add(bildirim);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTO
    public class OnayDTO
    {
        public int OnayAkisiId { get; set; }
        public bool Onaylandi { get; set; }
        public string? Aciklama { get; set; }
    }
}