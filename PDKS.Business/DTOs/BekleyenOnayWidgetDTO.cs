using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class BekleyenOnayWidgetDTO
    {
        
        public int SirketId { get; set; }
public int OnayKaydiId { get; set; }
        public string ModulTipi { get; set; }
        public string TalepEden { get; set; }
        public string AdimAdi { get; set; }
        public DateTime TalepTarihi { get; set; }
        public int BeklemeSuresi { get; set; }
        public string OncelikDurumu { get; set; }
    }
}
