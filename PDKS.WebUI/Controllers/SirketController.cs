// PDKS.WebUI/Controllers/SirketController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SirketController : BaseController
    {
        private readonly ISirketService _sirketService;

        public SirketController(ISirketService sirketService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _sirketService = sirketService;
        }

        // GET: Sirket/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var sirketler = await _sirketService.GetAllSirketlerAsync();
                return View(sirketler);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<SirketListDTO>());
            }
        }

        // GET: Sirket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var sirket = await _sirketService.GetSirketByIdAsync(id);
                if (sirket == null)
                {
                    TempData["Error"] = "Şirket bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(sirket);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Sirket/Create
        public async Task<IActionResult> Create()
        {
            // Ana şirket listesi için
            var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
            ViewBag.AnaSirketler = anaSirketler;
            return View();
        }

        // POST: Sirket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SirketCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                    ViewBag.AnaSirketler = anaSirketler;
                    return View(dto);
                }

                var sirketId = await _sirketService.CreateSirketAsync(dto);
                await LogAction("Ekleme", "Şirket", $"{dto.Unvan} şirketi eklendi");

                TempData["Success"] = "Şirket başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = anaSirketler;
                return View(dto);
            }
        }

        // GET: Sirket/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var sirket = await _sirketService.GetSirketByIdAsync(id);
                if (sirket == null)
                {
                    TempData["Error"] = "Şirket bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = anaSirketler;

                var dto = new SirketUpdateDTO
                {
                    Id = sirket.Id,
                    Unvan = sirket.Unvan,
                    TicariUnvan = sirket.TicariUnvan,
                    VergiNo = sirket.VergiNo,
                    VergiDairesi = sirket.VergiDairesi,
                    Telefon = sirket.Telefon,
                    Email = sirket.Email,
                    Adres = sirket.Adres,
                    Il = sirket.Il,
                    Ilce = sirket.Ilce,
                    PostaKodu = sirket.PostaKodu,
                    Website = sirket.Website,
                    LogoUrl = sirket.LogoUrl,
                    KurulusTarihi = sirket.KurulusTarihi,
                    Aktif = sirket.Aktif,
                    ParaBirimi = sirket.ParaBirimi,
                    Notlar = sirket.Notlar,
                    AnaSirket = sirket.AnaSirket,
                    AnaSirketId = sirket.AnaSirketId
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Sirket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SirketUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                    ViewBag.AnaSirketler = anaSirketler;
                    return View(dto);
                }

                await _sirketService.UpdateSirketAsync(dto);
                await LogAction("Güncelleme", "Şirket", $"{dto.Unvan} şirketi güncellendi");

                TempData["Success"] = "Şirket başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = anaSirketler;
                return View(dto);
            }
        }

        // POST: Sirket/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sirket = await _sirketService.GetSirketByIdAsync(id);
                await _sirketService.DeleteSirketAsync(id);
                await LogAction("Silme", "Şirket", $"{sirket.Unvan} şirketi silindi");

                TempData["Success"] = "Şirket başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Sirket/Switch
        [AllowAnonymous]
        public async Task<IActionResult> Switch()
        {
            var sirketler = await _sirketService.GetAllSirketlerAsync();
            return View(sirketler.Where(s => s.Aktif).ToList());
        }

        // POST: Sirket/SelectSirket
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SelectSirket(int sirketId)
        {
            HttpContext.Session.SetInt32("CurrentSirketId", sirketId);
            TempData["Success"] = "Şirket değiştirildi";
            return RedirectToAction("Index", "Home");
        }

        // PDKS.WebUI/Controllers/SirketController.cs içine ekle

        // GET: Sirket/BagliSirketler/5
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> BagliSirketler(int id)
        {
            try
            {
                var anaSirket = await _sirketService.GetSirketByIdAsync(id);

                if (anaSirket == null)
                {
                    TempData["Error"] = "Şirket bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                if (!anaSirket.AnaSirket)
                {
                    TempData["Error"] = "Bu şirket bir ana şirket değil";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var bagliSirketler = await _sirketService.GetBagliSirketlerAsync(id);

                ViewBag.AnaSirketId = id;
                ViewBag.AnaSirketAdi = anaSirket.Unvan;

                return View(bagliSirketler);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}