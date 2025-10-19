// PDKS.WebUI/Controllers/DepartmanController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK")]
    public class DepartmanController : BaseController
    {
        private readonly IDepartmanService _departmanService;
        private readonly ISirketService _sirketService;

        public DepartmanController(
            IDepartmanService departmanService,
            ISirketService sirketService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _departmanService = departmanService;
            _sirketService = sirketService;
        }

        // GET: Departman
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<DepartmanListDTO> departmanlar;

                if (User.IsInRole("Admin") && CurrentSirketId == 0)
                {
                    departmanlar = await _departmanService.GetAllAsync();
                }
                else
                {
                    departmanlar = await _departmanService.GetBySirketAsync(CurrentSirketId);
                }

                return View(departmanlar.ToList());
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

                // Şirket kontrolü (Admin hariç)
                if (!User.IsInRole("Admin") && departman.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu departmanı görüntüleme yetkiniz yok";
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
                await LoadSelectLists();
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
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.UstDepartmanId, dto.SirketId);
                    return View(dto);
                }

                // Şirket ID'yi otomatik ata (Admin değilse)
                if (!User.IsInRole("Admin"))
                {
                    dto.SirketId = CurrentSirketId;
                }

                await _departmanService.CreateAsync(dto);
                await LogAction("Ekleme", "Departman", $"{dto.DepartmanAdi} departmanı eklendi");

                TempData["Success"] = "Departman başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.UstDepartmanId, dto.SirketId);
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

                // Şirket kontrolü (Admin hariç)
                if (!User.IsInRole("Admin") && departman.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu departmanı düzenleme yetkiniz yok";
                    return RedirectToAction(nameof(Index));
                }

                await LoadSelectLists(departman.UstDepartmanId, departman.SirketId);

                var dto = new DepartmanUpdateDTO
                {
                    Id = departman.Id,
                    SirketId = departman.SirketId,
                    DepartmanAdi = departman.DepartmanAdi,
                    Kod = departman.Kod,
                    Aciklama = departman.Aciklama,
                    UstDepartmanId = departman.UstDepartmanId,
                    Durum = departman.Durum
                };

                return View(dto);
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
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.UstDepartmanId, dto.SirketId);
                    return View(dto);
                }

                // Şirket kontrolü (Admin hariç)
                var departman = await _departmanService.GetByIdAsync(dto.Id);
                if (!User.IsInRole("Admin") && departman.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu departmanı düzenleme yetkiniz yok";
                    return RedirectToAction(nameof(Index));
                }

                await _departmanService.UpdateAsync(dto);
                await LogAction("Güncelleme", "Departman", $"{dto.DepartmanAdi} departmanı güncellendi");

                TempData["Success"] = "Departman başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.UstDepartmanId, dto.SirketId);
                return View(dto);
            }
        }

        // POST: Departman/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var departman = await _departmanService.GetByIdAsync(id);

                await _departmanService.DeleteAsync(id);
                await LogAction("Silme", "Departman", $"{departman.DepartmanAdi} departmanı silindi");

                TempData["Success"] = "Departman başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        #region Helper Methods

        private async Task LoadSelectLists(int? ustDepartmanId = null, int? sirketId = null)
        {
            // Üst Departmanlar (Şirket bazlı)
            IEnumerable<DepartmanListDTO> departmanlar;

            if (User.IsInRole("Admin") && CurrentSirketId == 0)
            {
                departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
            }
            else
            {
                departmanlar = await _departmanService.GetBySirketAsync(CurrentSirketId);
            }

            ViewBag.UstDepartmanlar = new SelectList(departmanlar, "Id", "DepartmanAdi", ustDepartmanId);

            // Şirketler (Sadece Admin)
            if (User.IsInRole("Admin"))
            {
                var sirketler = await _sirketService.GetAllSirketlerAsync();
                ViewBag.Sirketler = new SelectList(
                    sirketler.Where(s => s.Aktif),
                    "Id",
                    "Unvan",
                    sirketId ?? CurrentSirketId
                );
            }
        }

        #endregion
    }
}