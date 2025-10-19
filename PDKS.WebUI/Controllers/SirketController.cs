// PDKS.WebUI/Controllers/SirketController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Ana şirketler listesi için SelectList oluştur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(anaSirketler, "Id", "Unvan");

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Sirket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SirketCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ı tekrar doldur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(anaSirketler, "Id", "Unvan", dto.AnaSirketId);
                return View(dto);
            }

            try
            {
                var sirketId = await _sirketService.CreateSirketAsync(dto);
                await LogAction("Ekleme", "Sirket", $"{dto.Unvan} şirketi eklendi");

                TempData["Success"] = "Şirket başarıyla eklendi";
                return RedirectToAction(nameof(Details), new { id = sirketId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ı tekrar doldur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(anaSirketler, "Id", "Unvan", dto.AnaSirketId);
                return View(dto);
            }
        }

        // GET: Sirket/Edit/5
        [Authorize(Roles = "Admin")]
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

                // Ana şirketler listesi için SelectList oluştur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(
                    anaSirketler.Where(s => s.Id != id), // Kendisi hariç
                    "Id",
                    "Unvan",
                    sirket.AnaSirketId
                );

                var updateDto = new SirketUpdateDTO
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

                return View(updateDto);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(SirketUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ı tekrar doldur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(
                    anaSirketler.Where(s => s.Id != dto.Id),
                    "Id",
                    "Unvan",
                    dto.AnaSirketId
                );
                return View(dto);
            }

            try
            {
                await _sirketService.UpdateSirketAsync(dto);
                await LogAction("Güncelleme", "Sirket", $"{dto.Unvan} şirketi güncellendi");

                TempData["Success"] = "Şirket başarıyla güncellendi";
                return RedirectToAction(nameof(Details), new { id = dto.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ı tekrar doldur
                var anaSirketler = await _sirketService.GetAnaSirketlerAsync();
                ViewBag.AnaSirketler = new SelectList(
                    anaSirketler.Where(s => s.Id != dto.Id),
                    "Id",
                    "Unvan",
                    dto.AnaSirketId
                );
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

        // PDKS.WebUI/Controllers/SirketController.cs - Ekle

        // GET: Sirket/Switch
        // PDKS.WebUI/Controllers/SirketController.cs - Switch GET
        [Authorize]
        public async Task<IActionResult> Switch()
        {
            try
            {
                // Kullanıcının erişebileceği şirketleri listele
                List<SirketListDTO> sirketler;

                if (User.IsInRole("Admin"))
                {
                    // Admin tüm şirketleri görebilir
                    sirketler = await _sirketService.GetAllSirketlerAsync();
                }
                else
                {
                    // Diğer kullanıcılar sadece kendi şirketini görür
                    var personel = await _unitOfWork.Personeller.GetByIdAsync(CurrentKullaniciId);
                    if (personel != null)
                    {
                        var sirket = await _sirketService.GetSirketByIdAsync(personel.SirketId);
                        sirketler = new List<SirketListDTO>
                {
                    new SirketListDTO
                    {
                        Id = sirket.Id,
                        Unvan = sirket.Unvan,
                        Aktif = sirket.Aktif,
                        AnaSirket = sirket.AnaSirket,
                        PersonelSayisi = sirket.PersonelSayisi
                    }
                };
                    }
                    else
                    {
                        sirketler = new List<SirketListDTO>();
                    }
                }

                ViewBag.CurrentSirketId = CurrentSirketId;  // int olarak gönder

                return View(sirketler);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Sirket/Switch
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Switch(int sirketId)
        {
            try
            {
                // Şirket var mı kontrol et
                var sirket = await _sirketService.GetSirketByIdAsync(sirketId);

                if (sirket == null)
                {
                    TempData["Error"] = "Şirket bulunamadı";
                    return RedirectToAction(nameof(Switch));
                }

                // Admin değilse, kendi şirketi olmalı
                if (!User.IsInRole("Admin"))
                {
                    var personel = await _unitOfWork.Personeller.GetByIdAsync(CurrentKullaniciId);
                    if (personel == null || personel.SirketId != sirketId)
                    {
                        TempData["Error"] = "Bu şirkete erişim yetkiniz yok";
                        return RedirectToAction(nameof(Switch));
                    }
                }

                // Yeni claim'ler oluştur
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                // Eski SirketId ve SirketAdi claim'lerini kaldır
                var oldSirketIdClaim = identity.FindFirst("SirketId");
                var oldSirketAdiClaim = identity.FindFirst("SirketAdi");

                if (oldSirketIdClaim != null)
                    identity.RemoveClaim(oldSirketIdClaim);

                if (oldSirketAdiClaim != null)
                    identity.RemoveClaim(oldSirketAdiClaim);

                // Yeni claim'leri ekle
                identity.AddClaim(new System.Security.Claims.Claim("SirketId", sirketId.ToString()));
                identity.AddClaim(new System.Security.Claims.Claim("SirketAdi", sirket.Unvan));

                // Cookie'yi güncelle
                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(identity));

                await LogAction("Şirket Değiştirme", "Sirket", $"{sirket.Unvan} şirketine geçiş yapıldı");

                TempData["Success"] = $"{sirket.Unvan} şirketine geçiş yapıldınız";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Switch));
            }
        }
    }
}