using PDKS.Business.Services;
using System.IO;

namespace PDKS.WebUI.BackgroundServices
{
    public class AutoBackupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AutoBackupBackgroundService> _logger;

        public AutoBackupBackgroundService(IServiceProvider serviceProvider, ILogger<AutoBackupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var backupService = scope.ServiceProvider.GetRequiredService<IBackupService>();

                        // Metot adını BackupDatabaseAsync olarak güncelledik.
                        // Bu metot artık bool yerine dosya yolunu string olarak döndürüyor.
                        var backupPath = await backupService.BackupDatabaseAsync();

                        if (!string.IsNullOrEmpty(backupPath))
                        {
                            _logger.LogInformation($"Otomatik veritabanı yedeği oluşturuldu: {backupPath}");
                        }
                        else
                        {
                            _logger.LogError("Otomatik veritabanı yedeği oluşturulamadı.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Otomatik yedekleme sırasında bir hata oluştu.");
                }

                // Her 24 saatte bir çalış
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}