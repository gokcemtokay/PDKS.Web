using System;
using System.Collections.Generic;

namespace PDKS.Business.DTOs
{
    public class ScheduledReportDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }
        public string ReportType { get; set; } // "GunlukRapor", "AylikRapor", vb.
        public string RecipientEmail { get; set; }
        public TimeSpan SendTime { get; set; } // 09:00, 17:00 gibi
        public List<DayOfWeek> SendDays { get; set; } // Pazartesi, Salı, vb.
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}