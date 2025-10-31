using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Devamsızlık Rapor DTO
    public class DevamsizlikRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string DevamsizlikNedeni { get; set; }
    }
}
