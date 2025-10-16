using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;


namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TatilController : Controller
    {
        private readonly ITatilService _tatilService;
        private readonly IUnitOfWork _unitOfWork;

        public TatilController(ITatilService tatilService, IUnitOfWork unitOfWork)
        {
            _tatilService = tatilService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var tatiller = await _tatilService.GetAllAsync();
            return View(tatiller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TatilCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Lütfen tüm alanları doldurun";
                    return RedirectToAction(nameof(Index));
                }

                await _tatilService.CreateAsync(dto);
                await LogAction("Ekleme", "Tatil", $"{dto.Ad} tatili eklendi");

                TempData["Success"] = "Tatil günü başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TatilUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Lütfen tüm alanları doldurun";
                    return RedirectToAction(nameof(Index));
                }

                await _tatilService.UpdateAsync(dto);
                await LogAction("Güncelleme", "Tatil", $"{dto.Ad} tatili güncellendi");

                TempData["Success"] = "Tatil günü başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tatil = await _tatilService.GetByIdAsync(id);
                await _tatilService.DeleteAsync(id);
                await LogAction("Silme", "Tatil", $"{tatil.Ad} tatili silindi");

                TempData["Success"] = "Tatil günü başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResmiTatilleriEkle()
        {
            try
            {
                var yil = DateTime.Now.Year;
                await _tatilService.ResmiTatilleriEkleAsync(yil);
                await LogAction("Toplu Ekleme", "Tatil", $"{yil} yılı resmi tatilleri eklendi");

                return Json(new { success = true, message = "Resmi tatiller başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task LogAction(string islem, string modul, string detay)
        {
            var kullaniciId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _unitOfWork.Loglar.AddAsync(new PDKS.Data.Entities.Log
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