using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using System.Security.Claims;

namespace PDKS.WebUI.Controllers.Mobile
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/mobile/[controller]")]
    [ApiController]
    public class MobileDashboardController : ControllerBase
    {
        private readonly PDKSDbContext _context;

        public MobileDashboardController(PDKSDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/mobile/MobileDashboard/summary
        [HttpGet("summary")]
        public async Task<ActionResult<MobileResponse<object>>> GetSummary()
        {
            var personelId = int.Parse(User.FindFirst("personelId")?.Value ?? "0");
            var bugun = DateTime.UtcNow.Date;
            var buAy = new DateTime(bugun.Year, bugun.Month, 1);

            try
            {
                // Bugünkü giriş-çıkış
                var bugunGiris = await _context.GirisCikislar
                    .Where(g => g.PersonelId == personelId &&
                               g.GirisZamani.HasValue &&
                               g.GirisZamani.Value.Date == bugun)
                    .OrderByDescending(g => g.GirisZamani)
                    .FirstOrDefaultAsync();

                // Bu ayki mesai - GirisCikis'ten hesapla
                var aylikGirisler = await _context.GirisCikislar
                    .Where(g => g.PersonelId == personelId &&
                               g.GirisZamani.HasValue &&
                               g.GirisZamani.Value >= buAy &&
                               g.GirisZamani.Value < buAy.AddMonths(1))
                    .ToListAsync();

                // Toplam çalışma dakikası hesapla
                int toplamDakika = 0;
                foreach (var giris in aylikGirisler)
                {
                    if (giris.GirisZamani.HasValue && giris.CikisZamani.HasValue)
                    {
                        var calismaZamani = giris.CikisZamani.Value - giris.GirisZamani.Value;
                        toplamDakika += (int)calismaZamani.TotalMinutes;
                    }
                }

                // Bekleyen onaylar
                //var bekleyenOnaylar = await _context.OnayAkislari
                //    .Where(o => o.OnaylayiciPersonelId == personelId && o.OnayDurumu == "Beklemede")
                //    .CountAsync();

                // Okunmamış bildirimler
                var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var okunmamisBildirimler = await _context.Bildirimler
                    .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                    .CountAsync();

                // İzin bakiyesi
                var izinHakki = await _context.IzinHaklari
                    .Where(ih => ih.PersonelId == personelId && ih.Yil == DateTime.UtcNow.Year)
                    .FirstOrDefaultAsync();

                return Ok(new MobileResponse<object>
                {
                    Success = true,
                    Message = "Dashboard özeti",
                    Data = new
                    {
                        BugunGiris = bugunGiris != null && bugunGiris.GirisZamani.HasValue ? new
                        {
                            GirisSaati = bugunGiris.GirisZamani.Value.ToString("HH:mm"),
                            CikisSaati = bugunGiris.CikisZamani?.ToString("HH:mm"),
                            ToplamSure = bugunGiris.CikisZamani.HasValue ?
                                (bugunGiris.CikisZamani.Value - bugunGiris.GirisZamani.Value).ToString(@"hh\:mm") : "Devam ediyor",
                            Durum = bugunGiris.Durum,
                            FazlaMesai = bugunGiris.FazlaMesaiSuresi > 0 ? $"{bugunGiris.FazlaMesaiSuresi} dk" : null
                        } : null,
                        AylikMesaiSaat = Math.Round((decimal)toplamDakika / 60, 1),
                        //BekleyenOnaylar = bekleyenOnaylar,
                        OkunmamisBildirimler = okunmamisBildirimler,
                        IzinBakiyesi = izinHakki?.KalanIzin ?? 0,
                        Tarih = DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MobileResponse<object>
                {
                    Success = false,
                    Message = "Bir hata oluştu: " + ex.Message,
                    Data = null,
                    ErrorCode = 500
                });
            }
        }

        // GET: api/v1/mobile/MobileDashboard/quick-actions
        [HttpGet("quick-actions")]
        public ActionResult<MobileResponse<object>> GetQuickActions()
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var actions = new List<object>
            {
                new { Id = "izin-talep", Icon = "calendar", Title = "İzin Talebi", Route = "/izin-talep" },
                new { Id = "giris-cikis", Icon = "clock", Title = "Giriş-Çıkış", Route = "/giris-cikis" },
                new { Id = "avans-talep", Icon = "money", Title = "Avans Talebi", Route = "/avans-talep" },
                new { Id = "masraf-talep", Icon = "receipt", Title = "Masraf Talebi", Route = "/masraf-talep" }
            };

            if (rol == "Yönetici" || rol == "IK" || rol == "Admin")
            {
                actions.Add(new { Id = "onay-bekleyen", Icon = "check-circle", Title = "Onay Bekleyenler", Route = "/onaylar" });
                actions.Add(new { Id = "personel-liste", Icon = "users", Title = "Personel Listesi", Route = "/personel" });
            }

            return Ok(new MobileResponse<object>
            {
                Success = true,
                Message = "Hızlı erişim menüsü",
                Data = actions
            });
        }

        // GET: api/v1/mobile/MobileDashboard/recent-activities
        [HttpGet("recent-activities")]
        public async Task<ActionResult<MobileResponse<object>>> GetRecentActivities()
        {
            var personelId = int.Parse(User.FindFirst("personelId")?.Value ?? "0");
            var sonBirHafta = DateTime.UtcNow.AddDays(-7);

            var activities = new List<object>();

            // Son giriş-çıkışlar
            var girisler = await _context.GirisCikislar
                .Where(g => g.PersonelId == personelId &&
                           g.GirisZamani.HasValue &&
                           g.GirisZamani.Value >= sonBirHafta)
                .OrderByDescending(g => g.GirisZamani)
                .Take(5)
                .ToListAsync();

            foreach (var g in girisler)
            {
                if (g.GirisZamani.HasValue)
                {
                    var description = $"{g.GirisZamani.Value:HH:mm} - {(g.CikisZamani.HasValue ? g.CikisZamani.Value.ToString("HH:mm") : "Devam ediyor")}";

                    if (g.GecKalmaSuresi > 0)
                        description += $" (Geç: {g.GecKalmaSuresi} dk)";
                    else if (g.FazlaMesaiSuresi > 0)
                        description += $" (Fazla: {g.FazlaMesaiSuresi} dk)";

                    activities.Add(new
                    {
                        Type = "giris-cikis",
                        Date = g.GirisZamani.Value,
                        Title = "Giriş-Çıkış",
                        Description = description,
                        Status = g.Durum
                    });
                }
            }

            // Son izin talepleri
            var izinler = await _context.Izinler
                .Where(i => i.PersonelId == personelId && i.BaslangicTarihi >= sonBirHafta)
                .OrderByDescending(i => i.BaslangicTarihi)
                .Take(3)
                .Select(i => new
                {
                    Type = "izin",
                    Date = i.BaslangicTarihi,
                    Title = "İzin Talebi",
                    Description = $"{i.IzinTipi} - {i.OnayDurumu}",
                    Status = i.OnayDurumu
                })
                .ToListAsync();

            activities.AddRange(izinler);

            var sortedActivities = activities
                .OrderByDescending(a => {
                    var prop = a.GetType().GetProperty("Date");
                    return prop != null ? (DateTime)prop.GetValue(a)! : DateTime.MinValue;
                })
                .Take(10);

            return Ok(new MobileResponse<object>
            {
                Success = true,
                Message = "Son aktiviteler",
                Data = sortedActivities
            });
        }
    }
}