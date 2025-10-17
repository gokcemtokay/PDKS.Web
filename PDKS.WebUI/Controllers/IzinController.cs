using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System.Security.Claims;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class IzinController : Controller
    {
        private readonly IIzinService _izinService;
        private readonly IPersonelService _personelService;

        public IzinController(IIzinService izinService, IPersonelService personelService)
        {
            _izinService = izinService;
            _personelService = personelService;
        }

        // GET: Izin
        public async Task<IActionResult> Index()
        {
            try
            {
                var izinler = await _izinService.GetAllAsync();
                return View(izinler);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<IzinListDTO>());
            }
        }

        // GET: Izin/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var izin = await _izinService.GetByIdAsync(id);
                if (izin == null)
                {
                    TempData["Error"] = "İzin kaydı bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(izin);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Izin/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Aktif personel listesi
                var personeller = await _personelService.GetActivePersonelsAsync();
                ViewBag.Personeller = new SelectList(personeller, "Id", "AdSoyad");

                // İzin tipleri
                ViewBag.IzinTipleri = new SelectList(new[]
                {
                    new { Value = "Yıllık", Text = "Yıllık İzin" },
                    new { Value = "Mazeret", Text = "Mazeret İzni" },
                    new { Value = "Hastalık", Text = "Hastalık İzni" },
                    new { Value = "Ücretsiz", Text = "Ücretsiz İzin" },
                    new { Value = "Evlilik", Text = "Evlilik İzni" },
                    new { Value = "Vefat", Text = "Vefat İzni" },
                    new { Value = "Doğum", Text = "Doğum İzni" }
                }, "Value", "Text");

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Izin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IzinCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(dto.PersonelId, dto.IzinTipi);
                return View(dto);
            }

            try
            {
                await _izinService.CreateAsync(dto);
                TempData["Success"] = "İzin talebi başarıyla oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadDropdownsAsync(dto.PersonelId, dto.IzinTipi);
                return View(dto);
            }
        }

        // GET: Izin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var izin = await _izinService.GetByIdAsync(id);
                if (izin == null)
                {
                    TempData["Error"] = "İzin kaydı bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                // Onaylanmış izin düzenlenemez
                if (izin.OnayDurumu == "Onaylandı")
                {
                    TempData["Error"] = "Onaylanmış izin kaydı düzenlenemez";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new IzinUpdateDTO
                {
                    Id = izin.Id,
                    PersonelId = izin.PersonelId,
                    IzinTipi = izin.IzinTipi,
                    BaslangicTarihi = izin.BaslangicTarihi,
                    BitisTarihi = izin.BitisTarihi,
                    Aciklama = izin.Aciklama
                };

                await LoadDropdownsAsync(izin.PersonelId, izin.IzinTipi);
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Izin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IzinUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(dto.PersonelId, dto.IzinTipi);
                return View(dto);
            }

            try
            {
                await _izinService.UpdateAsync(dto);
                TempData["Success"] = "İzin kaydı başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadDropdownsAsync(dto.PersonelId, dto.IzinTipi);
                return View(dto);
            }
        }

        // GET: Izin/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var izin = await _izinService.GetByIdAsync(id);
                if (izin == null)
                {
                    TempData["Error"] = "İzin kaydı bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(izin);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Izin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _izinService.DeleteAsync(id);
                TempData["Success"] = "İzin kaydı başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Izin/Approve/5
        [HttpPost]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _izinService.OnaylaAsync(id, kullaniciId);
                TempData["Success"] = "İzin talebi onaylandı";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Izin/Reject/5
        [HttpPost]
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> Reject(int id, string redNedeni)
        {
            try
            {
                var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _izinService.ReddetAsync(id, kullaniciId, redNedeni);
                TempData["Success"] = "İzin talebi reddedildi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Izin/Pending
        [Authorize(Roles = "Admin,IK,Yönetici")]
        public async Task<IActionResult> Pending()
        {
            try
            {
                var izinler = await _izinService.GetBekleyenIzinlerAsync();
                return View("Index", izinler);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Index", new List<IzinListDTO>());
            }
        }

        // Helper method
        private async Task LoadDropdownsAsync(int? selectedPersonelId = null, string? selectedIzinTipi = null)
        {
            var personeller = await _personelService.GetActivePersonelsAsync();
            ViewBag.Personeller = new SelectList(personeller, "Id", "AdSoyad", selectedPersonelId);

            ViewBag.IzinTipleri = new SelectList(new[]
            {
                new { Value = "Yıllık", Text = "Yıllık İzin" },
                new { Value = "Mazeret", Text = "Mazeret İzni" },
                new { Value = "Hastalık", Text = "Hastalık İzni" },
                new { Value = "Ücretsiz", Text = "Ücretsiz İzin" },
                new { Value = "Evlilik", Text = "Evlilik İzni" },
                new { Value = "Vefat", Text = "Vefat İzni" },
                new { Value = "Doğum", Text = "Doğum İzni" }
            }, "Value", "Text", selectedIzinTipi);
        }
    }
}