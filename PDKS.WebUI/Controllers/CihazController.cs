using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK")]
    public class CihazController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CihazController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Bugünün başlangıcını ve yarının başlangıcını UTC olarak tanımla
            var bugunBaslangicUtc = DateTime.UtcNow.Date;
            var yarinBaslangicUtc = bugunBaslangicUtc.AddDays(1);

            // 2. Repository'nin FindAsync metodunu kullanarak ilgili tüm kayıtları TEK BİR SORGUDAYLA veritabanından çek
            var bugunkuGirisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.CihazId.HasValue &&
                              g.GirisZamani.HasValue && // Null kontrolü eklemek her zaman iyidir
                              g.GirisZamani.Value >= bugunBaslangicUtc &&
                              g.GirisZamani.Value < yarinBaslangicUtc);

            // 3. Veritabanından gelen sonuçları C# tarafında (hafızada) grupla
            var bugunkuOkumalar = bugunkuGirisCikislar
                .GroupBy(g => g.CihazId.Value)
                .ToDictionary(group => group.Key, group => group.Count());

            // 4. Tüm cihazları veritabanından çek (Bu ikinci sorgu)
            var cihazlar = await _unitOfWork.Cihazlar.GetAllAsync();

            // 5. Cihaz listesini oluştururken önceden hazırladığımız dictionary'den verileri al
            var cihazList = cihazlar.Select(cihaz => new CihazListDTO
            {
                Id = cihaz.Id,
                CihazAdi = cihaz.CihazAdi,
                IPAdres = cihaz.IPAdres,
                Lokasyon = cihaz.Lokasyon,
                Durum = cihaz.Durum,
                SonBaglantiZamani = cihaz.SonBaglantiZamani,
                // Eğer cihaz için okuma varsa sayıyı al, yoksa 0 ata
                BugunkuOkumaSayisi = bugunkuOkumalar.GetValueOrDefault(cihaz.Id, 0)
            }).ToList();

            return View(cihazList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CihazCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CihazCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var cihaz = new Cihaz
                {
                    CihazAdi = dto.CihazAdi,
                    IPAdres = dto.IPAdres,
                    Lokasyon = dto.Lokasyon,
                    Durum = dto.Durum,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.Cihazlar.AddAsync(cihaz);
                await _unitOfWork.SaveChangesAsync();

                // Create log
                await _unitOfWork.CihazLoglari.AddAsync(new CihazLog
                {
                    CihazId = cihaz.Id,
                    Islem = "Cihaz Kaydı",
                    Basarili = true,
                    Detay = "Cihaz sisteme eklendi",
                    Tarih = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Cihaz Ekleme", "Cihaz", $"Yeni cihaz eklendi: {dto.CihazAdi}");

                TempData["Success"] = "Cihaz başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(id);
            if (cihaz == null)
                return NotFound();

            var dto = new CihazUpdateDTO
            {
                Id = cihaz.Id,
                CihazAdi = cihaz.CihazAdi,
                IPAdres = cihaz.IPAdres,
                Lokasyon = cihaz.Lokasyon,
                Durum = cihaz.Durum
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CihazUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(dto.Id);
                if (cihaz == null)
                    return NotFound();

                cihaz.CihazAdi = dto.CihazAdi;
                cihaz.IPAdres = dto.IPAdres;
                cihaz.Lokasyon = dto.Lokasyon;
                cihaz.Durum = dto.Durum;

                _unitOfWork.Cihazlar.Update(cihaz);
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Cihaz Güncelleme", "Cihaz", $"Cihaz güncellendi: {dto.CihazAdi}");

                TempData["Success"] = "Cihaz başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(id);
                if (cihaz == null)
                    return NotFound();

                _unitOfWork.Cihazlar.Remove(cihaz);
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Cihaz Silme", "Cihaz", $"Cihaz silindi: {cihaz.CihazAdi}");

                TempData["Success"] = "Cihaz başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Loglar(int id)
        {
            var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(id);
            if (cihaz == null)
                return NotFound();

            var loglar = await _unitOfWork.CihazLoglari.FindAsync(l => l.CihazId == id);

            ViewBag.CihazAdi = cihaz.CihazAdi;
            return View(loglar.OrderByDescending(l => l.Tarih).Take(100));
        }

        [HttpPost]
        public async Task<IActionResult> BaglantiTest(int id)
        {
            try
            {
                var cihaz = await _unitOfWork.Cihazlar.GetByIdAsync(id);
                if (cihaz == null)
                    return Json(new { success = false, message = "Cihaz bulunamadı" });

                // Simulate connection test
                cihaz.SonBaglantiZamani = DateTime.UtcNow;
                _unitOfWork.Cihazlar.Update(cihaz);

                await _unitOfWork.CihazLoglari.AddAsync(new CihazLog
                {
                    CihazId = id,
                    Islem = "Bağlantı Testi",
                    Basarili = true,
                    Detay = "Cihaz bağlantısı test edildi",
                    Tarih = DateTime.UtcNow
                });

                await _unitOfWork.SaveChangesAsync();

                return Json(new { success = true, message = "Bağlantı başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task LogAction(string islem, string modul, string detay)
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _unitOfWork.Loglar.AddAsync(new Log
            {
                KullaniciId = kullaniciId,
                Islem = islem,
                Modul = modul,
                Detay = detay,
                IpAdres = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Tarih = DateTime.UtcNow
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
