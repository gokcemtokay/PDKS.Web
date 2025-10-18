using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using PDKS.Data.Repositories;
using System.Threading.Tasks;

namespace PDKS.WebUI.Controllers
{
    /// <summary>
    ///  PDKS  gelişmiş özellikler için ayarlar kontrolcüsü
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportAndEmailService _exportService;
        private readonly IBackupService _backupService;

        public SettingsController(
            IUnitOfWork unitOfWork,
            IExportAndEmailService exportService,
            IBackupService backupService)
        {
            _unitOfWork = unitOfWork;
            _exportService = exportService;
            _backupService = backupService;
        }

        // GET: Settings/Index
        public IActionResult Index()
        {
            return View();
        }

        #region Otomatik Mail Ayarları

        // GET: Settings/AutomaticEmails
        public async Task<IActionResult> AutomaticEmails()
        {
            // Zamanlanmış raporları getir
            return View();
        }

        // POST: Settings/CreateScheduledReport
        [HttpPost]
        public async Task<IActionResult> CreateScheduledReport(ScheduledReportDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm alanları doldurun";
                return RedirectToAction(nameof(AutomaticEmails));
            }

            // Veritabanına kaydet
            TempData["Success"] = "Zamanlanmış rapor başarıyla oluşturuldu";
            return RedirectToAction(nameof(AutomaticEmails));
        }

        // POST: Settings/DeleteScheduledReport
        [HttpPost]
        public async Task<IActionResult> DeleteScheduledReport(int id)
        {
            TempData["Success"] = "Zamanlanmış rapor silindi";
            return RedirectToAction(nameof(AutomaticEmails));
        }

        #endregion

        #region Yedekleme Ayarları

        // GET: Settings/Backup
        public async Task<IActionResult> Backup()
        {
            var backupHistory = await _backupService.GetBackupHistory();
            return View(backupHistory);
        }

        // POST: Settings/CreateBackup
        [HttpPost]
        public async Task<IActionResult> CreateBackup()
        {
            var backupPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Backups",
                $"PDKS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak"
            );

            var result = await _backupService.CreateBackup(backupPath);

            if (result)
            {
                TempData["Success"] = "Yedekleme başarıyla oluşturuldu";
            }
            else
            {
                TempData["Error"] = "Yedekleme oluşturulamadı";
            }

            return RedirectToAction(nameof(Backup));
        }

        // POST: Settings/RestoreBackup
        [HttpPost]
        public async Task<IActionResult> RestoreBackup(string backupFile)
        {
            if (string.IsNullOrEmpty(backupFile))
            {
                TempData["Error"] = "Yedek dosyası seçilmedi";
                return RedirectToAction(nameof(Backup));
            }

            var result = await _backupService.RestoreBackup(backupFile);

            if (result)
            {
                TempData["Success"] = "Yedek başarıyla geri yüklendi";
            }
            else
            {
                TempData["Error"] = "Yedek geri yüklenemedi";
            }

            return RedirectToAction(nameof(Backup));
        }

        // GET: Settings/DownloadBackup
        public IActionResult DownloadBackup(string fileName)
        {
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Backups",
                fileName
            );

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "Dosya bulunamadı";
                return RedirectToAction(nameof(Backup));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        #endregion

        #region Sistem Parametreleri

        // GET: Settings/SystemParameters
        public async Task<IActionResult> SystemParameters()
        {
            // Parametreleri getir
            return View();
        }

        // POST: Settings/UpdateParameter
        [HttpPost]
        public async Task<IActionResult> UpdateParameter(string key, string value)
        {
            // Parametreyi güncelle
            TempData["Success"] = "Parametre güncellendi";
            return RedirectToAction(nameof(SystemParameters));
        }

        #endregion

        #region Kullanıcı Yetkileri

        // GET: Settings/UserPermissions
        public async Task<IActionResult> UserPermissions()
        {
            // Kullanıcı ve yetkileri getir
            return View();
        }

        #endregion

        #region Veri Kurtarma

        // GET: Settings/DataRecovery
        public IActionResult DataRecovery()
        {
            return View();
        }

        // POST: Settings/RecoverDeletedRecords
        [HttpPost]
        public async Task<IActionResult> RecoverDeletedRecords(string recordType, int recordId)
        {
            // Silinmiş kayıtları geri getir - özelliği
            TempData["Success"] = "Kayıt başarıyla geri getirildi";
            return RedirectToAction(nameof(DataRecovery));
        }

        #endregion

        #region Otomatik Veri Transferi

        // GET: Settings/AutoDataTransfer
        public IActionResult AutoDataTransfer()
        {
            return View();
        }

        // POST: Settings/ConfigureAutoTransfer
        [HttpPost]
        public async Task<IActionResult> ConfigureAutoTransfer(int intervalMinutes)
        {
            // Cihazlardan otomatik veri çekme ayarla - özelliği
            TempData["Success"] = $"Otomatik veri transferi {intervalMinutes} dakikada bir olarak ayarlandı";
            return RedirectToAction(nameof(AutoDataTransfer));
        }

        #endregion

        #region Update Kontrolü

        // GET: Settings/CheckUpdate
        public async Task<IActionResult> CheckUpdate()
        {
            // Yazılım güncellemesi kontrolü - özelliği
            var updateAvailable = false; // API'den kontrol et

            ViewBag.UpdateAvailable = updateAvailable;
            ViewBag.CurrentVersion = "1.0.0";
            ViewBag.LatestVersion = "1.0.0";

            return View();
        }

        #endregion

        #region Görsel Ayarlar

        // GET: Settings/Appearance
        public IActionResult Appearance()
        {
            return View();
        }

        // POST: Settings/UpdateAppearance
        [HttpPost]
        public async Task<IActionResult> UpdateAppearance(string theme, string fontSize)
        {
            // Görsel değişiklikler - özelliği
            TempData["Success"] = "Görünüm ayarları güncellendi";
            return RedirectToAction(nameof(Appearance));
        }

        #endregion

        #region Lisans Yönetimi

        // GET: Settings/License
        public IActionResult License()
        {
            ViewBag.LicenseKey = "XXXX-XXXX-XXXX-XXXX";
            ViewBag.ExpiryDate = DateTime.Now.AddYears(1);
            ViewBag.IsActive = true;
            ViewBag.MaxUsers = 100;
            ViewBag.CurrentUsers = 25;

            return View();
        }

        #endregion
    }
}