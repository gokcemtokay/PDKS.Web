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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK")]
    public class PersonelController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly IUnitOfWork _unitOfWork;

        public PersonelController(IPersonelService personelService, IUnitOfWork unitOfWork)
        {
            _personelService = personelService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var personeller = await _personelService.GetAllAsync();
            return View(personeller);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var personel = await _personelService.GetByIdAsync(id);
            if (personel == null)
                return NotFound();

            return View(personel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadViewBagData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonelCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                return View(dto);
            }

            try
            {
                var personelId = await _personelService.CreateAsync(dto);

                // Log the action
                await LogAction("Personel Ekleme", "Personel", $"Yeni personel eklendi: {dto.AdSoyad}");

                TempData["Success"] = "Personel başarıyla eklendi";
                return RedirectToAction(nameof(Details), new { id = personelId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadViewBagData();
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var personel = await _personelService.GetByIdAsync(id);
            if (personel == null)
                return NotFound();

            var dto = new PersonelUpdateDTO
            {
                Id = personel.Id,
                AdSoyad = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman,
                Gorev = personel.Gorev,
                Email = personel.Email,
                Telefon = personel.Telefon,
                Durum = personel.Durum,
                GirisTarihi = personel.GirisTarihi,
                CikisTarihi = personel.CikisTarihi,
                VardiyaId = personel.VardiyaId,
                Maas = personel.Maas,
                AvansLimiti = personel.AvansLimiti
            };

            await LoadViewBagData();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonelUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                return View(dto);
            }

            try
            {
                await _personelService.UpdateAsync(dto);

                // Log the action
                await LogAction("Personel Güncelleme", "Personel", $"Personel güncellendi: {dto.AdSoyad}");

                TempData["Success"] = "Personel başarıyla güncellendi";
                return RedirectToAction(nameof(Details), new { id = dto.Id });
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                await _personelService.DeleteAsync(id);

                // Log the action
                await LogAction("Personel Silme", "Personel", $"Personel pasif yapıldı: {personel.AdSoyad}");

                TempData["Success"] = "Personel başarıyla pasif yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var personeller = await _personelService.SearchAsync(searchTerm);
            return View("Index", personeller);
        }

        // Helper methods
        private async Task LoadViewBagData()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar.FindAsync(v => v.Durum);
            ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad");

            ViewBag.Departmanlar = new SelectList(new[]
            {
                "Bilgi İşlem",
                "İnsan Kaynakları",
                "Muhasebe",
                "Satış",
                "Pazarlama",
                "Üretim",
                "Lojistik",
                "Ar-Ge",
                "Kalite Kontrol",
                "Genel Müdürlük"
            });

            ViewBag.Gorevler = new SelectList(new[]
            {
                "Yazılım Geliştirici",
                "İK Uzmanı",
                "Muhasebe Uzmanı",
                "Satış Temsilcisi",
                "Pazarlama Müdürü",
                "Üretim Operatörü",
                "Lojistik Sorumlusu",
                "Ar-Ge Müdürü",
                "Kalite Kontrol Uzmanı",
                "Genel Müdür",
                "Yönetici",
                "Uzman",
                "Memur",
                "İşçi"
            });
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