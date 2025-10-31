using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Fazla Mesai Rapor DTO
    public class FazlaMesaiRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public int FazlaMesaiSuresi { get; set; }
        public string FazlaMesaiSuresiStr => $"{FazlaMesaiSuresi / 60}:{FazlaMesaiSuresi % 60:00}";
    }
}
