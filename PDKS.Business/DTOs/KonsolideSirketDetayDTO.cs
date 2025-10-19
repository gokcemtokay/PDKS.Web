using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class KonsolideSirketDetayDTO
    {
        public int SirketId { get; set; }
        public string SirketAdi { get; set; }
        public int PersonelSayisi { get; set; }
        public int DepartmanSayisi { get; set; }
        public decimal ToplamMaas { get; set; }
    }
}
