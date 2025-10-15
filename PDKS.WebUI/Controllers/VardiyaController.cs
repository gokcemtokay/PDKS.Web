using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK")]
    public class VardiyaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VardiyaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vardiyalar = await _unitOfWork.Vardiyalar.GetAllAsync();
            var vardiyaList = new List<VardiyaListDTO>();

            foreach (var vardiya in vardiyalar)
            {
                var personelSayisi = await _unitOfWork.Personeller.CountAsync(p =>
                    p.VardiyaId == vardiya.Id && p.Durum);

                vardiyaList.Add(new VardiyaListDTO
                {
                    Id = vardiya.Id,
                    Ad = vardiya.Ad,
                    BaslangicSaati = vardiya.BaslangicSaati,
                    BitisSaati = vardiya.BitisSaati,
                    GeceVardiyasiMi = vardiya.GeceVardiyasiMi,
                    EsnekVardiyaMi = vardiya.EsnekVardiyaMi,
                    ToleransSuresiDakika = vardiya.ToleransSuresiDakika,
                    Aciklama = vardiya.Aciklama,
                    Durum = vardiya.Durum,
                    PersonelSayisi = personelSayisi
                });
            }

            return View(vardiyaList.OrderBy(v => v.BaslangicSaati));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new VardiyaCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VardiyaCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var vardiya = new Vardiya
                {
                    Ad = dto.Ad,
                    BaslangicSaati = dto.BaslangicSaati,
                    BitisSaati = dto.BitisSaati,
                    GeceVardiyasiMi = dto.GeceVardiyasiMi,
                    EsnekVardiyaMi = dto.EsnekVardiyaMi,
                    ToleransSuresiDakika = dto.ToleransSuresiDakika,
                    Aciklama = dto.Aciklama,
                    Durum = dto.Durum
                };

                await _unitOfWork.Vardiyalar.AddAsync(vardiya);
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Vardiya Ekleme", "Vardiya", $"Yeni vardiya eklendi: {dto.Ad}");

                TempData["Success"] = "Vardiya başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);
            if (vardiya == null)
                return NotFound();

            var dto = new VardiyaUpdateDTO
            {
                Id = vardiya.Id,
                Ad = vardiya.Ad,
                BaslangicSaati = vardiya.BaslangicSaati,
                BitisSaati = vardiya.BitisSaati,
                GeceVardiyasiMi = vardiya.GeceVardiyasiMi,
                EsnekVardiyaMi = vardiya.EsnekVardiyaMi,
                ToleransSuresiDakika = vardiya.ToleransSuresiDakika,
                Aciklama = vardiya.Aciklama,
                Durum = vardiya.Durum
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VardiyaUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(dto.Id);
                if (vardiya == null)
                    return NotFound();

                vardiya.Ad = dto.Ad;
                vardiya.BaslangicSaati = dto.BaslangicSaati;
                vardiya.BitisSaati = dto.BitisSaati;
                vardiya.GeceVardiyasiMi = dto.GeceVardiyasiMi;
                vardiya.EsnekVardiyaMi = dto.EsnekVardiyaMi;
                vardiya.ToleransSuresiDakika = dto.ToleransSuresiDakika;
                vardiya.Aciklama = dto.Aciklama;
                vardiya.Durum = dto.Durum;

                _unitOfWork.Vardiyalar.Update(vardiya);
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Vardiya Güncelleme", "Vardiya", $"Vardiya güncellendi: {dto.Ad}");

                TempData["Success"] = "Vardiya başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(id);
                if (vardiya == null)
                    return NotFound();

                // Check if any personnel is using this shift
                var personelSayisi = await _unitOfWork.Personeller.CountAsync(p => p.VardiyaId == id);
                if (personelSayisi > 0)
                {
                    TempData["Error"] = "Bu vardiyayı kullanan personel bulunmaktadır. Önce personelleri başka vardiyaya aktarın.";
                    return RedirectToAction(nameof(Index));
                }

                _unitOfWork.Vardiyalar.Remove(vardiya);
                await _unitOfWork.SaveChangesAsync();

                await LogAction("Vardiya Silme", "Vardiya", $"Vardiya silindi: {vardiya.Ad}");

                TempData["Success"] = "Vardiya başarıyla silindi";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LogAction(string islem, string modul, string detay)
        {
            var kullaniciId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _unitOfWork.Loglar.AddAsync(new Log
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
