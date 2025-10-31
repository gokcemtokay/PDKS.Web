using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Geç Kalanlar Rapor DTO
    public class GecKalanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string PersonelAdi { get; set; } // ReportService için eklendi
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string Departman { get; set; } // ReportService için eklendi
        public TimeSpan VardiyaBaslangic { get; set; }
        public string BeklenenGiris { get; set; } // ReportService için eklendi
        public DateTime? GirisSaati { get; set; }
        public string GercekGiris { get; set; } // ReportService için eklendi
        public int GecKalmaSuresi { get; set; }
        public string GecKalmaSuresiStr => $"{GecKalmaSuresi / 60}:{GecKalmaSuresi % 60:00}";
    }
}
