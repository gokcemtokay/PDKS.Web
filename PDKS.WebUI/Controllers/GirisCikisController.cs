using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class GirisCikisController : Controller
    {
        private readonly IGirisCikisService _girisCikisService;
        private readonly IUnitOfWork _unitOfWork;

        public GirisCikisController(IGirisCikisService girisCikisService, IUnitOfWork unitOfWork)
        {
            _girisCikisService = girisCikisService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? baslangic, DateTime? bitis)
        {
            var basDate = baslangic ?? DateTime.Today.AddDays(-30);
            var bitDate = bitis ?? DateTime.Today;

            var kayitlar = await _girisCikisService.GetByDateRangeAsync(basDate, bitDate);

            // If not admin/ik, show only own records
            if (!User.IsInRole("Admin") && !User.IsInRole("IK"))
            {
                var personelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");
                kayitlar = kayitlar.Where(k => k.PersonelId == personelId);
            }

            ViewBag.BaslangicTarihi = basDate.ToString("yyyy-MM-dd");
            ViewBag.BitisTarihi = bitDate.ToString("yyyy-MM-dd");

            return View(kayitlar);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Create()
        {
            await LoadViewBagData();
            return View(new GirisCikisCreateDTO { GirisZamani = DateTime.UtcNow });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Create(GirisCikisCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                return View(dto);
            }

            try
            {
                dto.ElleGiris = true; // Manual entry
                await _girisCikisService.CreateAsync(dto);

                // Log the action
                await LogAction("Manuel Giriş-Çıkış Kaydı", "GirisCikis",
                    $"Personel ID: {dto.PersonelId} için manuel kayıt eklendi");

                TempData["Success"] = "Giriş-çıkış kaydı başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadViewBagData();
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Edit(int id)
        {
            var kayit = await _girisCikisService.GetByIdAsync(id);
            if (kayit == null)
                return NotFound();

            var dto = new GirisCikisUpdateDTO
            {
                Id = kayit.Id,
                PersonelId = kayit.PersonelId,
                GirisZamani = kayit.GirisZamani,
                CikisZamani = kayit.CikisZamani,
                Kaynak = kayit.Kaynak,
                CihazId = kayit.CihazId,
                ElleGiris = kayit.ElleGiris,
                Not = kayit.Not
            };

            await LoadViewBagData();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Edit(GirisCikisUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                return View(dto);
            }

            try
            {
                await _girisCikisService.UpdateAsync(dto);

                // Log the action
                await LogAction("Giriş-Çıkış Güncelleme", "GirisCikis",
                    $"ID: {dto.Id} kayıt güncellendi");

                TempData["Success"] = "Giriş-çıkış kaydı başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadViewBagData();
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _girisCikisService.DeleteAsync(id);

                // Log the action
                await LogAction("Giriş-Çıkış Silme", "GirisCikis", $"ID: {id} kayıt silindi");

                TempData["Success"] = "Kayıt başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var kayit = await _girisCikisService.GetByIdAsync(id);
            if (kayit == null)
                return NotFound();

            // Check permission
            if (!User.IsInRole("Admin") && !User.IsInRole("IK"))
            {
                var personelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");
                if (kayit.PersonelId != personelId)
                    return Forbid();
            }

            return View(kayit);
        }

        [HttpGet]
        public async Task<IActionResult> MyAttendance()
        {
            var personelId = int.Parse(User.FindFirst("PersonelId")?.Value ?? "0");
            var kayitlar = await _girisCikisService.GetByPersonelAsync(personelId);

            return View(kayitlar.OrderByDescending(k => k.GirisZamani).Take(30));
        }

        // Helper methods
        private async Task LoadViewBagData()
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
            ViewBag.Personeller = new SelectList(personeller, "Id", "AdSoyad");

            var cihazlar = await _unitOfWork.Cihazlar.FindAsync(c => c.Durum);
            ViewBag.Cihazlar = new SelectList(cihazlar, "Id", "CihazAdi");
        }

        private async Task LogAction(string islem, string modul, string detay)
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _unitOfWork.Loglar.AddAsync(new Data.Entities.Log
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