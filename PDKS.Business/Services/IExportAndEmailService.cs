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
    public interface IExportAndEmailService
    {
        Task<byte[]> ExportToExcel<T>(List<T> data, string sheetName);
        Task<bool> SendReportByEmail(string toEmail, string subject, byte[] attachment, string fileName);
        Task<bool> ScheduleAutomaticReport(string reportType, string email, TimeSpan sendTime);
    
        //Task<IEnumerable<ExportAndEmailListDTO>> GetBySirketAsync(int sirketId);
}
}
