using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class KPIDTO
    {
        public string Baslik { get; set; }
        public double Deger { get; set; }
        public double Hedef { get; set; }
        public string Birim { get; set; }
        public string Trend { get; set; } // "up", "down", "stable"
    }
}
