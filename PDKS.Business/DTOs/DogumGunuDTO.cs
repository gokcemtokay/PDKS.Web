using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class DogumGunuDTO
    {
        public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string ProfilFoto { get; set; }
        public DateTime DogumTarihi { get; set; }
        public int KacGunSonra { get; set; }
        public bool Bugun { get; set; }
    }
}
