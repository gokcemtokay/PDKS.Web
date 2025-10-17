using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class DepartmanController : Controller
    {
        private readonly IDepartmanService _departmanService;

        public DepartmanController(IDepartmanService departmanService)
        {
            _departmanService = departmanService;
        }

        // GET: Departman
        public async Task<IActionResult> Index()
        {
            try
            {
                var departmanlar = await _departmanService.GetAllAsync();
                return View(departmanlar);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<DepartmanListDTO>());
            }
        }

        // GET: Departman/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);
                if (departman == null)
                {
                    TempData["Error"] = "Departman bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(departman);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Departman/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Üst departman listesi için
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(departmanlar, "Id", "Ad");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Departman/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmanCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ı tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(departmanlar, "Id", "Ad", dto.UstDepartmanId);
                return View(dto);
            }

            try
            {
                await _departmanService.CreateAsync(dto);
                TempData["Success"] = "Departman başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ı tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(departmanlar, "Id", "Ad", dto.UstDepartmanId);
                return View(dto);
            }
        }

        // GET: Departman/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);
                if (departman == null)
                {
                    TempData["Error"] = "Departman bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new DepartmanUpdateDTO
                {
                    Id = departman.Id,
                    Ad = departman.Ad,
                    Kod = departman.Kod,
                    Aciklama = departman.Aciklama,
                    UstDepartmanId = departman.UstDepartmanId,
                    Durum = departman.Durum
                };

                // Üst departman listesi (kendisi hariç)
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(
                    departmanlar.Where(d => d.Id != id),
                    "Id",
                    "Ad",
                    updateDto.UstDepartmanId
                );

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Departman/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmanUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ı tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(
                    departmanlar.Where(d => d.Id != dto.Id),
                    "Id",
                    "Ad",
                    dto.UstDepartmanId
                );
                return View(dto);
            }

            try
            {
                await _departmanService.UpdateAsync(dto);
                TempData["Success"] = "Departman başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ı tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.UstDepartmanlar = new SelectList(
                    departmanlar.Where(d => d.Id != dto.Id),
                    "Id",
                    "Ad",
                    dto.UstDepartmanId
                );
                return View(dto);
            }
        }

        // GET: Departman/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);
                if (departman == null)
                {
                    TempData["Error"] = "Departman bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(departman);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Departman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departmanService.DeleteAsync(id);
                TempData["Success"] = "Departman başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Departman/Active
        public async Task<IActionResult> Active()
        {
            try
            {
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                return View("Index", departmanlar);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Index", new List<DepartmanListDTO>());
            }
        }
    }
}