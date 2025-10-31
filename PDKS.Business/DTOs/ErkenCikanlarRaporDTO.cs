using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class ErkenCikanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string PersonelAdi { get; set; } // ReportService için eklendi
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string Departman { get; set; } // ReportService için eklendi
        public TimeSpan VardiyaBitis { get; set; }
        public string BeklenenCikis { get; set; } // ReportService için eklendi
        public DateTime? CikisSaati { get; set; }
        public string GercekCikis { get; set; } // ReportService için eklendi
        public int ErkenCikisSuresi { get; set; }
        public string ErkenCikisSuresiStr => $"{ErkenCikisSuresi / 60}:{ErkenCikisSuresi % 60:00}";
    }
}
