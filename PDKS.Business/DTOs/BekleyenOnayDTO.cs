using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class BekleyenOnayDTO
    {
        public int OnayKaydiId { get; set; }
        public string ModulTipi { get; set; }
        public int ReferansId { get; set; }
        public string TalepEdenKisi { get; set; }
        public string TalepEdenDepartman { get; set; }
        public DateTime TalepTarihi { get; set; }
        public string AdimAdi { get; set; }
        public int BeklemeSuresi { get; set; } // Kaç gündür bekliyor
        public string OncelikDurumu { get; set; } // "Normal", "Acil" (timeout'a yakın)
    }
}
