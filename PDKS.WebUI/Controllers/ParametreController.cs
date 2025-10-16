using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;


namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParametreController : Controller
    {
        private readonly IParametreService _parametreService;
        private readonly IUnitOfWork _unitOfWork;

        public ParametreController(IParametreService parametreService, IUnitOfWork unitOfWork)
        {
            _parametreService = parametreService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var parametreler = await _parametreService.GetAllAsync();
            return View(parametreler);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParametreCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Lütfen tüm zorunlu alanları doldurun";
                    return RedirectToAction(nameof(Index));
                }

                await _parametreService.CreateAsync(dto);
                await LogAction("Ekleme", "Parametre", $"{dto.Ad} parametresi eklendi");

                TempData["Success"] = "Parametre başarıyla eklendi";
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
        public async Task<IActionResult> Edit(ParametreUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Lütfen tüm zorunlu alanları doldurun";
                    return RedirectToAction(nameof(Index));
                }

                await _parametreService.UpdateAsync(dto);
                await LogAction("Güncelleme", "Parametre", $"{dto.Ad} parametresi güncellendi");

                TempData["Success"] = "Parametre başarıyla güncellendi";
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
                var parametre = await _parametreService.GetByIdAsync(id);
                await _parametreService.DeleteAsync(id);
                await LogAction("Silme", "Parametre", $"{parametre.Ad} parametresi silindi");

                TempData["Success"] = "Parametre başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
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