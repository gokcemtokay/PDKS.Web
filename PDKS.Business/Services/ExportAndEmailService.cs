using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;

namespace PDKS.Business.Services
{

    public class ExportAndEmailService : IExportAndEmailService
    {
        private readonly IConfiguration _configuration;

        public ExportAndEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generic Excel export 
        /// </summary>
        public async Task<byte[]> ExportToExcel<T>(List<T> data, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Başlık satırı
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#667eea");
                worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
            }

            // Veri satırları
            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item);
                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                }
            }

            // Otomatik genişlik ayarla
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return await Task.FromResult(stream.ToArray());
        }

        /// <summary>
        /// Otomatik mail gönderme - PDKS özelliği
        /// </summary>
        public async Task<bool> SendReportByEmail(string toEmail, string subject, byte[] attachment, string fileName)
        {
            try
            {
                // appsettings.json'dan mail ayarları
                var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var password = _configuration["EmailSettings:Password"];
                var displayName = _configuration["EmailSettings:DisplayName"] ?? "PDKS Sistemi";

                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(password))
                {
                    throw new Exception("Email ayarları yapılandırılmamış. appsettings.json kontrol edin.");
                }

                using var mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, displayName);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = $@"
Merhaba,

{subject} raporunuz ektedir.

Bu mail otomatik olarak PDKS Sistemi tarafından gönderilmiştir.

---
PDKS - Personel Devam Kontrol Sistemi
{DateTime.Now:dd.MM.yyyy HH:mm}
";
                mail.IsBodyHtml = false;

                if (attachment != null && attachment.Length > 0)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(attachment), fileName));
                }

                using var smtp = new SmtpClient(smtpHost, smtpPort);
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email gönderme hatası: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ScheduleAutomaticReport(string reportType, string email, TimeSpan sendTime)
        {
            return await Task.FromResult(true);
        }
    }
}