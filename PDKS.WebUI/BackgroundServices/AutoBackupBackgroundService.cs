using PDKS.Business.Services;

namespace PDKS.WebUI.BackgroundServices
{
    public class AutoBackupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AutoBackupBackgroundService> _logger;

        public AutoBackupBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<AutoBackupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Otomatik Yedekleme Servisi başlatıldı");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    // Her gün saat 02:00'de yedek al
                    if (now.Hour == 2 && now.Minute == 0)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();

                        var backupPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "Backups",
                            $"Auto_Backup_{now:yyyyMMdd_HHmmss}.bak"
                        );

                        var result = await backupService.CreateBackup(backupPath);

                        if (result)
                        {
                            _logger.LogInformation($"Otomatik yedekleme başarılı: {backupPath}");

                            // Eski yedekleri temizle (30 günden eski)
                            CleanOldBackups();
                        }
                        else
                        {
                            _logger.LogError("Otomatik yedekleme başarısız");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Otomatik yedekleme sırasında hata oluştu");
                }

                // Her 1 dakikada bir kontrol et
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private void CleanOldBackups()
        {
            try
            {
                var backupDir = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
                if (!Directory.Exists(backupDir)) return;

                var files = Directory.GetFiles(backupDir, "*.bak");
                var cutoffDate = DateTime.Now.AddDays(-30);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                        _logger.LogInformation($"Eski yedek silindi: {fileInfo.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eski yedekleri temizlerken hata oluştu");
            }
        }
    }
}