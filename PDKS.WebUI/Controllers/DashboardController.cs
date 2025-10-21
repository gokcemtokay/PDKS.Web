using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public DashboardController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/Dashboard
        [HttpGet]
        public async Task<ActionResult<object>> GetDashboard([FromQuery] int sirketId)
        {
            var bugun = DateTime.UtcNow.Date;
            var ay = DateTime.UtcNow.Month;
            var yil = DateTime.UtcNow.Year;

            // Bugün işe gelenler
            var bugunIste = await _context.GirisCikislar
                .Where(g => g.GirisZamani.HasValue && g.GirisZamani.Value.Date == bugun && g.CikisZamani == null) // ✅ Düzeltildi
                .Select(g => g.PersonelId)
                .Distinct()
                .CountAsync();

            // Bugün izinli olanlar
            var bugunIzinli = await _context.Izinler
                .Where(i => i.BaslangicTarihi <= bugun && i.BitisTarihi >= bugun && i.OnayDurumu == "Onaylandi")
                .CountAsync();

            // Geç kalanlar (bugün)
            var gecKalanlar = await _context.GirisCikislar
                .Include(g => g.Personel)
                .ThenInclude(p => p.Vardiya)
                .Where(g => g.GirisZamani.HasValue && g.GirisZamani.Value.Date == bugun) // ✅ Düzeltildi
                .Select(g => new
                {
                    PersonelId = g.PersonelId,
                    PersonelAdi = g.Personel.AdSoyad, // ✅ Düzeltildi
                    GirisSaati = g.GirisZamani.HasValue ? g.GirisZamani.Value.TimeOfDay : TimeSpan.Zero, // ✅ Düzeltildi
                    VardiyaBaslangic = g.Personel.Vardiya != null ? g.Personel.Vardiya.BaslangicSaati : TimeSpan.Zero,
                    Gecikme = g.GirisZamani.HasValue && g.Personel.Vardiya != null
                        ? g.GirisZamani.Value.TimeOfDay - g.Personel.Vardiya.BaslangicSaati
                        : TimeSpan.Zero // ✅ Düzeltildi
                })
                .Where(x => x.Gecikme > TimeSpan.FromMinutes(15))
                .ToListAsync();

            // Doğum günü olanlar (bu hafta)
            var haftaBaslangici = bugun.AddDays(-(int)bugun.DayOfWeek);
            var haftaSonu = haftaBaslangici.AddDays(7);

            var dogumGunuOlanlar = await _context.Personeller
                .Where(p => p.DogumTarihi.Month == bugun.Month && // ✅ Düzeltildi (DateTime nullable değilse)
                           p.DogumTarihi.Day >= bugun.Day &&
                           p.DogumTarihi.Day <= haftaSonu.Day)
                .Select(p => new
                {
                    p.Id,
                    Adi = p.AdSoyad, // ✅ Düzeltildi
                    DogumGunu = p.DogumTarihi.Day + "/" + p.DogumTarihi.Month // ✅ Düzeltildi
                })
                .ToListAsync();

            // Son giriş/çıkışlar (son 10)
            var sonHareketler = await _context.GirisCikislar
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.GirisZamani.HasValue) // ✅ Null kontrolü
                .OrderByDescending(g => g.GirisZamani)
                .Take(10)
                .Select(g => new
                {
                    PersonelAdi = g.Personel.AdSoyad, // ✅ Düzeltildi
                    Islem = g.CikisZamani == null ? "Giriş" : "Çıkış",
                    Zaman = g.CikisZamani ?? g.GirisZamani,
                    CihazAdi = g.Cihaz != null ? g.Cihaz.CihazAdi : "Manuel"
                })
                .ToListAsync();

            // Bekleyen onaylar
            var bekleyenOnaylar = await _context.OnayAkislari
                .Where(o => o.OnayDurumu == "Beklemede")
                .CountAsync();

            return Ok(new
            {
                BugunIsteSayisi = bugunIste,
                BugunIzinliSayisi = bugunIzinli,
                GecKalanSayisi = gecKalanlar.Count,
                GecKalanlar = gecKalanlar.Take(5),
                DogumGunuSayisi = dogumGunuOlanlar.Count,
                DogumGunuOlanlar = dogumGunuOlanlar,
                SonHareketler = sonHareketler,
                BekleyenOnaylar = bekleyenOnaylar
            });
        }

        // GET: api/Dashboard/AylikTrend
        [HttpGet("AylikTrend")]
        public async Task<ActionResult<object>> GetAylikTrend([FromQuery] int sirketId)
        {
            var bugun = DateTime.UtcNow.Date;
            var ayBaslangic = new DateTime(bugun.Year, bugun.Month, 1);

            var enCokGecKalanlar = await _context.GirisCikislar
                .Include(g => g.Personel)
                .ThenInclude(p => p.Vardiya)
                .Where(g => g.GirisZamani.HasValue && g.GirisZamani.Value >= ayBaslangic) // ✅ Düzeltildi
                .GroupBy(g => g.PersonelId)
                .Select(group => new
                {
                    PersonelId = group.Key,
                    PersonelAdi = group.First().Personel.AdSoyad, // ✅ Düzeltildi
                    GecKalmaSayisi = group.Count(g =>
                        g.Personel.Vardiya != null &&
                        g.GirisZamani.HasValue &&
                        g.GirisZamani.Value.TimeOfDay > g.Personel.Vardiya.BaslangicSaati.Add(TimeSpan.FromMinutes(15))) // ✅ Düzeltildi
                })
                .OrderByDescending(x => x.GecKalmaSayisi)
                .Take(10)
                .ToListAsync();

            return Ok(enCokGecKalanlar);
        }
    }
}