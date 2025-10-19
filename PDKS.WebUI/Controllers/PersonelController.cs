// PDKS.WebUI/Controllers/PersonelController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;

namespace PDKS.WebUI.Controllers
{
    [Authorize]
    public class PersonelController : BaseController
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmanService _departmanService;
        private readonly IVardiyaService _vardiyaService;
        private readonly ISirketService _sirketService;

        public PersonelController(
            IPersonelService personelService,
            IDepartmanService departmanService,
            IVardiyaService vardiyaService,
            ISirketService sirketService,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _personelService = personelService;
            _departmanService = departmanService;
            _vardiyaService = vardiyaService;
            _sirketService = sirketService;
        }

        // GET: Personel
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<PersonelListDTO> personeller;

                // Admin tüm şirketleri görebilir
                if (User.IsInRole("Admin") && CurrentSirketId == 0)
                {
                    personeller = await _personelService.GetAllAsync();
                }
                else
                {
                    // Diğer kullanıcılar sadece kendi şirketlerini görür
                    personeller = await _personelService.GetBySirketAsync(CurrentSirketId);
                }

                return View(personeller.ToList());
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

                // Şirket kontrolü (Admin hariç)
                if (!User.IsInRole("Admin") && personel.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu personeli görüntüleme yetkiniz yok";
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
                await LoadSelectLists();
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
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.DepartmanId, dto.VardiyaId, dto.SirketId);
                    return View(dto);
                }

                // Şirket ID'yi otomatik ata (Admin değilse)
                if (!User.IsInRole("Admin"))
                {
                    dto.SirketId = CurrentSirketId;
                }

                var personelId = await _personelService.CreateAsync(dto);
                await LogAction("Ekleme", "Personel", $"{dto.AdSoyad} personeli eklendi");

                TempData["Success"] = "Personel başarıyla eklendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.DepartmanId, dto.VardiyaId, dto.SirketId);
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

                // Şirket kontrolü (Admin hariç)
                if (!User.IsInRole("Admin") && personel.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu personeli düzenleme yetkiniz yok";
                    return RedirectToAction(nameof(Index));
                }

                await LoadSelectLists(personel.DepartmanId, personel.VardiyaId, personel.SirketId);

                var dto = new PersonelUpdateDTO
                {
                    Id = personel.Id,
                    SirketId = personel.SirketId,
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
                    DepartmanId = personel.DepartmanId,
                    VardiyaId = personel.VardiyaId,
                    AvansLimiti = personel.AvansLimiti,
                    Durum = personel.Durum,
                    Notlar = personel.Notlar
                };

                return View(dto);
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
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadSelectLists(dto.DepartmanId, dto.VardiyaId, dto.SirketId);
                    return View(dto);
                }

                // Şirket kontrolü (Admin hariç)
                var personel = await _personelService.GetByIdAsync(dto.Id);
                if (!User.IsInRole("Admin") && personel.SirketId != CurrentSirketId)
                {
                    TempData["Error"] = "Bu personeli düzenleme yetkiniz yok";
                    return RedirectToAction(nameof(Index));
                }

                await _personelService.UpdateAsync(dto);
                await LogAction("Güncelleme", "Personel", $"{dto.AdSoyad} personeli güncellendi");

                TempData["Success"] = "Personel başarıyla güncellendi";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                await LoadSelectLists(dto.DepartmanId, dto.VardiyaId, dto.SirketId);
                return View(dto);
            }
        }

        // POST: Personel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);

                await _personelService.DeleteAsync(id);
                await LogAction("Silme", "Personel", $"{personel.AdSoyad} personeli silindi");

                TempData["Success"] = "Personel başarıyla silindi (pasif edildi)";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Personel/Transfer/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Transfer(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);

                if (personel == null)
                {
                    TempData["Error"] = "Personel bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                // Tüm şirketleri getir
                var sirketler = await _sirketService.GetAllSirketlerAsync();
                ViewBag.Sirketler = new SelectList(
                    sirketler.Where(s => s.Id != personel.SirketId && s.Aktif),
                    "Id",
                    "Unvan"
                );

                ViewBag.PersonelAdSoyad = personel.AdSoyad;
                ViewBag.MevcutSirket = personel.SirketAdi;

                var dto = new PersonelTransferDTO
                {
                    PersonelId = id,
                    EskiSirketId = personel.SirketId,
                    TransferTarihi = DateTime.Now
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Personel/Transfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Transfer(PersonelTransferDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var personel = await _personelService.GetByIdAsync(dto.PersonelId);
                    var sirketler = await _sirketService.GetAllSirketlerAsync();
                    ViewBag.Sirketler = new SelectList(
                        sirketler.Where(s => s.Id != personel.SirketId && s.Aktif),
                        "Id",
                        "Unvan"
                    );
                    ViewBag.PersonelAdSoyad = personel.AdSoyad;
                    ViewBag.MevcutSirket = personel.SirketAdi;
                    return View(dto);
                }

                var success = await _sirketService.TransferPersonelAsync(dto, CurrentKullaniciId);

                if (success)
                {
                    await LogAction("Transfer", "Personel",
                        $"Personel (ID:{dto.PersonelId}) şirketler arası transfer edildi");
                    TempData["Success"] = "Personel başarıyla transfer edildi";
                }
                else
                {
                    TempData["Error"] = "Transfer işlemi başarısız";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Personel/TransferHistory/5
        [Authorize(Roles = "Admin,IK")]
        public async Task<IActionResult> TransferHistory(int id)
        {
            try
            {
                var personel = await _personelService.GetByIdAsync(id);

                if (personel == null)
                {
                    TempData["Error"] = "Personel bulunamadı";
                    return RedirectToAction(nameof(Index));
                }

                var transferGecmisi = await _sirketService.GetPersonelTransferGecmisiAsync(id);

                ViewBag.PersonelAdSoyad = personel.AdSoyad;
                ViewBag.PersonelId = id;

                return View(transferGecmisi);
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

        #region Helper Methods

        private async Task LoadSelectLists(int? departmanId = null, int? vardiyaId = null, int? sirketId = null)
        {
            // Departmanlar (Şirket bazlı)
            IEnumerable<DepartmanListDTO> departmanlar;

            if (User.IsInRole("Admin") && CurrentSirketId == 0)
            {
                departmanlar = await _departmanService.GetAllAsync();
            }
            else
            {
                departmanlar = await _departmanService.GetBySirketAsync(CurrentSirketId);
            }

            ViewBag.Departmanlar = new SelectList(departmanlar, "Id", "DepartmanAdi", departmanId);

            // Vardiyalar
            var vardiyalar = await _vardiyaService.GetAllAsync();
            ViewBag.Vardiyalar = new SelectList(vardiyalar, "Id", "Ad", vardiyaId);

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