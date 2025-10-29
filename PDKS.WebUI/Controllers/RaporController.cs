using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using ClosedXML.Excel;
using System.IO;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin,IK,Yönetici")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RaporController : ControllerBase
    {
        private readonly IReportService _reportService;

        public RaporController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Yardımcı metot: JWT token'dan aktif şirket ID'sini alır.
        private int GetCurrentSirketId()
        {
            var sirketIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sirketId");
            if (sirketIdClaim != null && int.TryParse(sirketIdClaim.Value, out int sirketId))
            {
                return sirketId;
            }
            throw new UnauthorizedAccessException("Yetkilendirme token'ında şirket ID'si bulunamadı.");
        }


        #region Giriş-Çıkış Raporları

        [HttpPost("kisi-bazinda-giris-cikis")]
        public async Task<IActionResult> KisiBazindaGirisCikis([FromBody] RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue)
                return BadRequest("Personel seçimi zorunludur.");

            var rapor = await _reportService.KisiBazindaGirisCikisRaporu(filtre.PersonelId.Value, filtre.BaslangicTarihi, filtre.BitisTarihi);
            return Ok(rapor);
        }

        [HttpPost("genel-giris-cikis")]
        public async Task<IActionResult> GenelGirisCikis([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGirisCikisRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);
            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }
            return Ok(rapor);
        }

        #endregion

        #region Geç Kalanlar Raporları

        [HttpPost("kisi-bazinda-gec-kalanlar")]
        public async Task<IActionResult> KisiBazindaGecKalanlar([FromBody] RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue)
                return BadRequest("Personel seçimi zorunludur.");

            var rapor = await _reportService.KisiBazindaGecKalanlarRaporu(filtre.PersonelId.Value, filtre.BaslangicTarihi, filtre.BitisTarihi);
            return Ok(rapor);
        }

        [HttpPost("genel-gec-kalanlar")]
        public async Task<IActionResult> GenelGecKalanlar([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGecKalanlarRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);
            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }
            return Ok(rapor);
        }

        #endregion

        #region Diğer Raporlar

        [HttpPost("mesaiye-kalanlar")]
        public async Task<IActionResult> MesaiyeKalanlar([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.MesaiyeKalanlarRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);
            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }
            return Ok(rapor);
        }

        [HttpPost("devamsizlar")]
        public async Task<IActionResult> Devamsizlar([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.DevamsizlarRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);
            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }
            return Ok(rapor);
        }

        [HttpPost("izinli-personeller")]
        public async Task<IActionResult> IzinliPersoneller([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.IzinliPersonellerRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);
            if (!string.IsNullOrEmpty(filtre.Departman))
            {
                rapor = rapor.Where(r => r.Departman == filtre.Departman).ToList();
            }
            return Ok(rapor);
        }

        // ... Diğer tüm rapor metotları benzer şekilde POST olarak devam edebilir ...

        #endregion

        #region Maaş ve Prim Raporları

        [HttpPost("kisi-bazinda-maas-bordrosu")]
        public async Task<IActionResult> KisiBazindaMaasBordrosu([FromBody] RaporFiltreDTO filtre)
        {
            if (!filtre.PersonelId.HasValue || !filtre.Yil.HasValue || !filtre.Ay.HasValue)
                return BadRequest("Personel, Yıl ve Ay seçimi zorunludur.");

            var rapor = await _reportService.KisiBazindaMaasBordrosu(filtre.PersonelId.Value, filtre.Yil.Value, filtre.Ay.Value);
            return Ok(rapor);
        }

        [HttpPost("genel-maas-bordrosu")]
        public async Task<IActionResult> GenelMaasBordrosu([FromBody] RaporFiltreDTO filtre)
        {
            if (!filtre.Yil.HasValue || !filtre.Ay.HasValue)
                return BadRequest("Yıl ve Ay seçimi zorunludur.");

            var rapor = await _reportService.GenelBazdaMaasBordrosu(filtre.Yil.Value, filtre.Ay.Value);
            return Ok(rapor);
        }

        #endregion

        #region Excel Export

        [HttpPost("export/giris-cikis-excel")]
        public async Task<IActionResult> ExportGirisCikisExcel([FromBody] RaporFiltreDTO filtre)
        {
            var rapor = await _reportService.GenelBazdaGirisCikisRaporu(filtre.BaslangicTarihi, filtre.BitisTarihi);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Giriş-Çıkış Raporu");

            worksheet.Cell(1, 1).Value = "Tarih";
            worksheet.Cell(1, 2).Value = "Personel Adı";
            worksheet.Cell(1, 3).Value = "Sicil No";
            worksheet.Cell(1, 4).Value = "Departman";
            worksheet.Cell(1, 5).Value = "Giriş Zamanı";
            worksheet.Cell(1, 6).Value = "Çıkış Zamanı";
            worksheet.Cell(1, 7).Value = "Çalışma Süresi";
            worksheet.Cell(1, 8).Value = "Durum";

            var headerRange = worksheet.Range(1, 1, 1, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

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

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"GirisCikisRaporu_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        #endregion
    }
}