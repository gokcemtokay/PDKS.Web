using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PDKS.Business.Services;
using PDKS.Data.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using PDKS.Business.DTOs;

namespace PDKS.WebUI.BackgroundServices
{
    /// <summary>
    /// Zamanlanmış raporları otomatik olarak gönderen background service
    /// Dosya konumu: PDKS.WebUI/BackgroundServices/ScheduledReportBackgroundService.cs
    /// </summary>
    public class ScheduledReportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledReportBackgroundService> _logger;

        public ScheduledReportBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ScheduledReportBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Zamanlanmış Rapor Servisi başlatıldı - {time}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var exportService = scope.ServiceProvider.GetRequiredService<IExportAndEmailService>();

                    var now = DateTime.Now;

                    // TODO: Veritabanından zamanlanmış raporları getir
                    // Örnek kullanım:
                    // var scheduledReports = await unitOfWork.ScheduledReports
                    //     .FindAsync(r => r.IsActive && 
                    //                     r.SendTime.Hours == now.Hour && 
                    //                     r.SendTime.Minutes == now.Minute &&
                    //                     r.SendDays.Contains(now.DayOfWeek));

                    // foreach (var report in scheduledReports)
                    // {
                    //     await GenerateAndSendReport(report, exportService, unitOfWork);
                    // }

                    _logger.LogInformation("Zamanlanmış raporlar kontrol edildi: {time}", now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Zamanlanmış rapor gönderiminde hata oluştu");
                }

                // Her 1 dakikada bir kontrol et
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Zamanlanmış Rapor Servisi durduruldu");
        }

        private async Task GenerateAndSendReport(
            ScheduledReportDTO scheduledReport, // ✅ dynamic yerine ScheduledReportDTO
            IExportAndEmailService exportService,
            IUnitOfWork unitOfWork)
        {
            try
            {
                _logger.LogInformation($"Rapor oluşturuluyor: {scheduledReport.ReportType}");

                // Rapor türüne göre veriyi hazırla
                byte[] excelData = null;
                string fileName = $"{scheduledReport.ReportType}_{DateTime.Now:yyyyMMdd}.xlsx";

                switch (scheduledReport.ReportType)
                {
                    case "GunlukGirisCikis":
                        // Günlük rapor oluştur
                        break;
                    case "GenelGecKalanlar":
                        // Geç kalanlar raporu oluştur
                        break;
                        // Diğer rapor türleri...
                }

                if (excelData != null)
                {
                    // Mail gönder
                    var subject = $"{scheduledReport.ReportType} - {DateTime.Now:dd.MM.yyyy}";
                    await exportService.SendReportByEmail(
                        scheduledReport.RecipientEmail,
                        subject,
                        excelData,
                        fileName
                    );

                    _logger.LogInformation($"Rapor başarıyla gönderildi: {scheduledReport.RecipientEmail}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Rapor oluşturma/gönderme hatası: {scheduledReport.ReportType}");
            }
        }
    }
}