using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class YilDonumuDTO
    {
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string ProfilFoto { get; set; }
        public DateTime GirisTarihi { get; set; }
        public int KacYil { get; set; }
        public int KacGunSonra { get; set; }
        public bool Bugun { get; set; }
    }
}
