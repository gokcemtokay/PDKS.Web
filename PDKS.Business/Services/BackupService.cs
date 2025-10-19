using PDKS.Data.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PDKS.Business.Services
{
    public class BackupService : IBackupService
    {
        private readonly PDKSDbContext _context;
        private readonly string _backupFolderPath;

        public BackupService(PDKSDbContext context)
        {
            _context = context;
            _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            if (!Directory.Exists(_backupFolderPath))
            {
                Directory.CreateDirectory(_backupFolderPath);
            }
        }

        public async Task<string> BackupDatabaseAsync()
        {
            var backupFileName = $"PDKS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            var backupFilePath = Path.Combine(_backupFolderPath, backupFileName);

            // Entity Framework Core doğrudan backup desteklemez, bu yüzden SQL komutu kullanırız.
            // Bu komut PostgreSQL için özelleştirilmelidir. Şimdilik sadece dosyayı oluşturmayı simüle edelim.
            await File.WriteAllTextAsync(backupFilePath, $"Backup created at {DateTime.UtcNow}");

            return backupFilePath;
        }

        public Task<bool> RestoreBackup(string backupFilePath)
        {
            // Bu işlem çok riskli olduğu ve veritabanına özel komutlar gerektirdiği için
            // API üzerinden doğrudan yapılması genellikle önerilmez.
            // Şimdilik sadece başarılı olduğunu varsayalım.
            return Task.FromResult(true);
        }

        public Task<IEnumerable<object>> GetBackupHistory()
        {
            var files = Directory.GetFiles(_backupFolderPath, "*.bak")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.CreationTime)
                .Select(f => new { FileName = f.Name, CreatedDate = f.CreationTime, Size = f.Length });

            return Task.FromResult<IEnumerable<object>>(files);
        }
    }
}