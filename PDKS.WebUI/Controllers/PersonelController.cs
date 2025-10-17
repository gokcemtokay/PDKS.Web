using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class PersonelController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmanService _departmanService;
        private readonly IVardiyaService _vardiyaService;

        public PersonelController(
            IPersonelService personelService,
            IDepartmanService departmanService,
            IVardiyaService vardiyaService)
        {
            _personelService = personelService;
            _departmanService = departmanService;
            _vardiyaService = vardiyaService;
        }

        // GET: Personel
        public async Task<IActionResult> Index()
        {
            try
            {
                var personeller = await _personelService.GetAllAsync();
                return View(personeller);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<PersonelListDTO>());
            }
        }

        // GET: Personel/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    TempData["Error"] = "Personel bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(personel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Personel/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Departman listesini ViewBag'e ekle
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad");

                // Vardiya listesini ViewBag'e ekle
                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad");

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Personel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonelCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ları tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad", dto.DepartmanId);

                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", dto.VardiyaId);

                return View(dto);
            }

            try
            {
                await _personelService.CreateAsync(dto);
                TempData["Success"] = "Personel başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ları tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad", dto.DepartmanId);

                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", dto.VardiyaId);

                return View(dto);
            }
        }

        // GET: Personel/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    TempData["Error"] = "Personel bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                // PersonelDetailDTO'dan PersonelUpdateDTO'ya mapping
                var updateDto = new PersonelUpdateDTO
                {
                    Id = personel.Id,
                    AdSoyad = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    TcKimlikNo = personel.TcKimlikNo,
                    Email = personel.Email,
                    Telefon = personel.Telefon,
                    Adres = personel.Adres,
                    DogumTarihi = personel.DogumTarihi,
                    Cinsiyet = personel.Cinsiyet,
                    KanGrubu = personel.KanGrubu,
                    GirisTarihi = personel.GirisTarihi,
                    CikisTarihi = personel.CikisTarihi,
                    Maas = personel.Maas,
                    Unvan = personel.Unvan,
                    Gorev = personel.Gorev,
                    AvansLimiti = personel.AvansLimiti,
                    DepartmanId = personel.DepartmanId,
                    Departman = personel.DepartmanAdi,
                    VardiyaId = personel.VardiyaId,
                    Durum = personel.Durum,
                    Notlar = personel.Notlar
                };

                // Departman listesini ViewBag'e ekle
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad", updateDto.DepartmanId);

                // Vardiya listesini ViewBag'e ekle
                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", updateDto.VardiyaId);

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Personel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonelUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Hata durumunda dropdown'ları tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad", dto.DepartmanId);

                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", dto.VardiyaId);

                return View(dto);
            }

            try
            {
                await _personelService.UpdateAsync(dto);
                TempData["Success"] = "Personel başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                // Hata durumunda dropdown'ları tekrar doldur
                var departmanlar = await _departmanService.GetAktifDepartmanlarAsync();
                ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "Ad", dto.DepartmanId);

                var vardiyalar = await _vardiyaService.GetAktifVardiyalarAsync();
                ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", dto.VardiyaId);

                return View(dto);
            }
        }

        // GET: Personel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);
                if (personel == null)
                {
                    TempData["Error"] = "Personel bulunamadı";
                    return RedirectToAction(nameof(Index));
                }
                return View(personel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Personel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _personelService.DeleteAsync(id);
                TempData["Success"] = "Personel başarıyla silindi (pasif edildi)";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Personel/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                var personeller = await _personelService.SearchAsync(searchTerm);
                return View("Index", personeller);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Index", new List<PersonelListDTO>());
            }
        }

        // GET: Personel/Active
        public async Task<IActionResult> Active()
        {
            try
            {
                var personeller = await _personelService.GetActivePersonelsAsync();
                return View("Index", personeller);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Index", new List<PersonelListDTO>());
            }
        }
    }
}