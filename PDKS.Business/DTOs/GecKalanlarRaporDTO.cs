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
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public TimeSpan VardiyaBaslangic { get; set; }
        public DateTime? GirisSaati { get; set; }
        public int GecKalmaSuresi { get; set; }
        public string GecKalmaSuresiStr => $"{GecKalmaSuresi / 60}:{GecKalmaSuresi % 60:00}";
    }
}
