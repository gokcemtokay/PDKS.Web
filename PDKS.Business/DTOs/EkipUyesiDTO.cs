using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class EkipUyesiDTO
    {
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string ProfilFoto { get; set; }
        public string Durum { get; set; } // "Aktif", "Izinli", "Raporlu"
        public DateTime? SonGiris { get; set; }
    }
}
