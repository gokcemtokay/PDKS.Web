using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PDKS.Business.DTOs;
using PDKS.Business.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace PDKS.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
#if DEBUG
    [Route("api/[controller]")] // ⬅️ Development: /api/auth/login
#else
[Route("[controller]")] // ⬅️ Production: /auth/login (IIS /api ekler)
#endif
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SettingsController : ControllerBase
    {
        private readonly IBackupService _backupService;
        private readonly IParametreService _parametreService;
        // Diğer servisler de buraya eklenebilir.

        public SettingsController(IBackupService backupService, IParametreService parametreService)
        {
            _backupService = backupService;
            _parametreService = parametreService;
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


        #region Yedekleme Ayarları

        [HttpGet("backup-history")]
        public async Task<IActionResult> GetBackupHistory()
        {
            var backupHistory = await _backupService.GetBackupHistory();
            return Ok(backupHistory);
        }

        [HttpPost("create-backup")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupPath = await _backupService.BackupDatabaseAsync();
                return Ok(new { message = "Yedekleme başarıyla oluşturuldu.", filePath = backupPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Yedekleme oluşturulamadı: {ex.Message}");
            }
        }

        [HttpPost("restore-backup")]
        public async Task<IActionResult> RestoreBackup([FromBody] BackupRequestDTO dto)
        {
            if (string.IsNullOrEmpty(dto.FileName))
                return BadRequest("Yedek dosyası seçilmedi.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Backups", dto.FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            var result = await _backupService.RestoreBackup(filePath);
            if (result)
                return Ok(new { message = "Yedek başarıyla geri yüklendi." });
            else
                return StatusCode(500, "Yedek geri yüklenemedi.");
        }

        [HttpGet("download-backup/{fileName}")]
        public IActionResult DownloadBackup(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Backups", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        #endregion

        #region Sistem Parametreleri

        // Bu işlevsellik zaten `ParametreController`'a taşındığı için burada tekrar edilmesine gerek yok.
        // React uygulaması `api/parametre` endpoint'ini kullanacaktır.

        #endregion

        #region Lisans Yönetimi

        [HttpGet("license")]
        public IActionResult License()
        {
            // Bu bilgiler genellikle daha güvenli bir yerden okunmalıdır.
            var licenseInfo = new
            {
                LicenseKey = "XXXX-XXXX-XXXX-XXXX",
                ExpiryDate = DateTime.Now.AddYears(1),
                IsActive = true,
                MaxUsers = 100,
                CurrentUsers = 25
            };
            return Ok(licenseInfo);
        }

        #endregion

        // Diğer özellikler (DataRecovery, AutoDataTransfer, CheckUpdate, Appearance vb.)
        // henüz servis katmanında bir karşılığı olmadığı için şimdilik eklenmemiştir.
        // Bu özellikler için ayrı servisler yazıldığında, buraya benzer şekilde API endpoint'leri eklenebilir.
    }

    // Yedekleme ve Geri Yükleme için DTO
    public class BackupRequestDTO
    {
        public string FileName { get; set; }
    }
}