using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;


namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciController(IKullaniciService kullaniciService, IUnitOfWork unitOfWork)
        {
            _kullaniciService = kullaniciService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _kullaniciService.GetAllAsync();
            return View(kullanicilar);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KullaniciCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.PersonelId, dto.RolId);
                    return View(dto);
                }

                await _kullaniciService.CreateAsync(dto);
                await LogAction("Ekleme", "Kullanıcı", $"{dto.KullaniciAdi} kullanıcısı eklendi");

                TempData["Success"] = "Kullanıcı başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.PersonelId, dto.RolId);
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var kullanici = await _kullaniciService.GetByIdAsync(id);
                var dto = new KullaniciUpdateDTO
                {
                    Id = kullanici.Id,
                    KullaniciAdi = kullanici.KullaniciAdi,
                    PersonelId = kullanici.PersonelId,
                    RolId = kullanici.RolId,
                    Aktif = kullanici.Aktif
                };

                await LoadSelectLists(dto.PersonelId, dto.RolId);
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KullaniciUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.PersonelId, dto.RolId);
                    return View(dto);
                }

                await _kullaniciService.UpdateAsync(dto);
                await LogAction("Güncelleme", "Kullanıcı", $"{dto.KullaniciAdi} kullanıcısı güncellendi");

                TempData["Success"] = "Kullanıcı başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.PersonelId, dto.RolId);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var kullanici = await _kullaniciService.GetByIdAsync(id);
                await _kullaniciService.DeleteAsync(id);
                await LogAction("Silme", "Kullanıcı", $"{kullanici.KullaniciAdi} kullanıcısı silindi");

                TempData["Success"] = "Kullanıcı başarıyla silindi";
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
        public async Task<IActionResult> SifreDegistir(int id, string yeniSifre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(yeniSifre) || yeniSifre.Length < 6)
                {
                    TempData["Error"] = "Şifre en az 6 karakter olmalıdır";
                    return RedirectToAction(nameof(Index));
                }

                await _kullaniciService.SifreDegistirAsync(id, yeniSifre);

                var kullanici = await _kullaniciService.GetByIdAsync(id);
                await LogAction("Şifre Değiştirme", "Kullanıcı", $"{kullanici.KullaniciAdi} kullanıcısının şifresi değiştirildi");

                TempData["Success"] = "Şifre başarıyla değiştirildi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task LoadSelectLists(int? selectedPersonelId = null, int? selectedRolId = null)
        {
            var personeller = await _unitOfWork.Personeller.GetAllAsync();
            var roller = await _unitOfWork.Roller.GetAllAsync();

            // Sadece henüz kullanıcısı olmayan personeller
            var kullanicilar = await _unitOfWork.Kullanicilar.GetAllAsync();
            var personelIdsWithUser = kullanicilar.Where(k => k.PersonelId.HasValue).Select(k => k.PersonelId.Value).ToList();

            var availablePersoneller = personeller.Where(p =>
                !personelIdsWithUser.Contains(p.Id) || p.Id == selectedPersonelId);

            ViewBag.Personeller = new SelectList(
                availablePersoneller.Select(p => new { p.Id, DisplayText = $"{p.AdSoyad} ({p.SicilNo})" }),
                "Id",
                "DisplayText",
                selectedPersonelId
            );

            ViewBag.Roller = new SelectList(roller, "Id", "RolAdi", selectedRolId);
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