using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;
using ClosedXML.Excel;
using System.IO;
using System.Drawing;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK,Yönetici")]
    public class RaporController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUnitOfWork _unitOfWork;

        public RaporController(IReportService reportService, IUnitOfWork unitOfWork)
        {
            _reportService = reportService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await LoadViewBagData();
            return View();
        }

        #region Giriş-Çıkış Raporları

        [HttpGet]
        public async Task<IActionResult> KisiBazindaGirisCikis()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> KisiBazindaGirisCikis(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue)
            {
                TempData["Error"] = "Personel seçimi zorunludur";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.KisiBazindaGirisCikisRaporu(
                filtre.PersonelId.Value,
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> GenelGirisCikis()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> GenelGirisCikis(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGirisCikisRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        #endregion

        #region Geç Kalanlar Raporları

        [HttpGet]
        public async Task<IActionResult> KisiBazindaGecKalanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> KisiBazindaGecKalanlar(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue)
            {
                TempData["Error"] = "Personel seçimi zorunludur";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.KisiBazindaGecKalanlarRaporu(
                filtre.PersonelId.Value,
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> GenelGecKalanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> GenelGecKalanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGecKalanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        #endregion

        #region Erken Çıkanlar Raporları

        [HttpGet]
        public async Task<IActionResult> KisiBazindaErkenCikanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> KisiBazindaErkenCikanlar(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue)
            {
                TempData["Error"] = "Personel seçimi zorunludur";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.KisiBazindaErkenCikanlarRaporu(
                filtre.PersonelId.Value,
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> GenelErkenCikanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> GenelErkenCikanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaErkenCikanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        #endregion

        #region Diğer Raporlar

        [HttpGet]
        public async Task<IActionResult> MesaiyeKalanlar()
        {
            await LoadViewBagData();
            var viewModel = new MesaiyeKalanlarViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MesaiyeKalanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.MesaiyeKalanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            //var viewModel = new MesaiyeKalanlarViewModel
            //{
            //    Filtre = filtre, // Kullanıcının formda doldurduğu filtre
            //    RaporSonuclari = await _reportService.MesaiyeKalanlarRaporu(filtre.BaslangicTarihi,
            //    filtre.BitisTarihi)
            //};

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> Devamsizlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Devamsizlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.DevamsizlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> IzinliPersoneller()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> IzinliPersoneller(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.IzinliPersonellerRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> TatilGunuCalisanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> TatilGunuCalisanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.TatilGunuCalisanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> ElleGirisRaporu()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ElleGirisRaporu(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.ElleGirisRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> KartUnutanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> KartUnutanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.KartUnutanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> IseGirenler()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> IseGirenler(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.IseGirenlerRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> IstenAyrilanlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> IstenAyrilanlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.IstenAyrilanlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> NotluKayitlar()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> NotluKayitlar(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.NotluKayitlarRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        #endregion

        #region Maaş ve Prim Raporları

        [HttpGet]
        public async Task<IActionResult> KisiBazindaMaasBordrosu()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> KisiBazindaMaasBordrosu(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue || !filtre.Yil.HasValue || !filtre.Ay.HasValue)
            {
                TempData["Error"] = "Tüm alanları doldurunuz";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.KisiBazindaMaasBordrosu(
                filtre.PersonelId.Value,
                filtre.Yil.Value,
                filtre.Ay.Value);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> GenelMaasBordrosu()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> GenelMaasBordrosu(RaporFiltreDTO filtre)
        {
            if (!filtre.Yil.HasValue || !filtre.Ay.HasValue)
            {
                TempData["Error"] = "Yıl ve ay seçimi zorunludur";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.GenelBazdaMaasBordrosu(
                filtre.Yil.Value,
                filtre.Ay.Value);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> AvansListesi()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AvansListesi(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.AvansListesi(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> MaasZarfi()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> MaasZarfi(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue || !filtre.Yil.HasValue || !filtre.Ay.HasValue)
            {
                TempData["Error"] = "Tüm alanları doldurunuz";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.MaasZarfi(
                filtre.PersonelId.Value,
                filtre.Yil.Value,
                filtre.Ay.Value);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> PrimListesi()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> PrimListesi(RaporFiltreDTO filtre)
        {
            if (!filtre.Yil.HasValue || !filtre.Ay.HasValue)
            {
                TempData["Error"] = "Yıl ve ay seçimi zorunludur";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.PrimListesi(
                filtre.Yil.Value,
                filtre.Ay.Value);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        [HttpGet]
        public async Task<IActionResult> AylikDevamCizelgesi()
        {
            await LoadViewBagData();
            return View(new RaporFiltreDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AylikDevamCizelgesi(RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue || !filtre.Yil.HasValue || !filtre.Ay.HasValue)
            {
                TempData["Error"] = "Tüm alanları doldurunuz";
                await LoadViewBagData();
                return View(filtre);
            }

            var rapor = await _reportService.AylikDevamCizelgesi(
                filtre.PersonelId.Value,
                filtre.Yil.Value,
                filtre.Ay.Value);

            ViewBag.Rapor = rapor;
            ViewBag.Filtre = filtre;
            await LoadViewBagData();
            return View(filtre);
        }

        #endregion

        #region Excel Export

        [HttpPost]
        public async Task<IActionResult> ExportGirisCikisExcel(RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGirisCikisRaporu(
                filtre.BaslangicTarihi,
                filtre.BitisTarihi);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Giriş-Çıkış Raporu");

            // Header
            worksheet.Cell(1, 1).Value = "Tarih";
            worksheet.Cell(1, 2).Value = "Personel Adı";
            worksheet.Cell(1, 3).Value = "Sicil No";
            worksheet.Cell(1, 4).Value = "Departman";
            worksheet.Cell(1, 5).Value = "Giriş Zamanı";
            worksheet.Cell(1, 6).Value = "Çıkış Zamanı";
            worksheet.Cell(1, 7).Value = "Çalışma Süresi";
            worksheet.Cell(1, 8).Value = "Durum";

            // Style header
            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            int row = 2;
            foreach (var item in rapor)
            {
                worksheet.Cell(row, 1).Value = item.Tarih.ToString("dd.MM.yyyy");
                worksheet.Cell(row, 2).Value = item.PersonelAdi;
                worksheet.Cell(row, 3).Value = item.SicilNo;
                worksheet.Cell(row, 4).Value = item.Departman;
                worksheet.Cell(row, 5).Value = item.GirisSaati?.ToString("HH:mm") ?? "-";
                worksheet.Cell(row, 6).Value = item.CikisSaati?.ToString("HH:mm") ?? "-";
                worksheet.Cell(row, 7).Value = item.ToplamCalismaSuresi;
                worksheet.Cell(row, 8).Value = item.Durum;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"GirisCikisRaporu_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        #endregion

        private async Task LoadViewBagData()
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
            ViewBag.Personeller = new SelectList(personeller, "Id", "AdSoyad");

            var departmanlar = (await _unitOfWork.Departmanlar.GetAllAsync())
                .Where(d => !string.IsNullOrEmpty(d.Ad)) 
                .Select(d => d.Ad)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
            ViewBag.Departmanlar = new SelectList(departmanlar);

            ViewBag.Yillar = new SelectList(Enumerable.Range(DateTime.UtcNow.Year - 5, 10).Reverse());
            ViewBag.Aylar = new SelectList(Enumerable.Range(1, 12).Select(m => new
            {
                Value = m,
                Text = new DateTime(2000, m, 1).ToString("MMMM")
            }), "Value", "Text");
        }
    }
}