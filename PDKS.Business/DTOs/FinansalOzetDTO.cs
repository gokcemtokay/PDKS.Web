using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class FinansalOzetDTO
    {
        public decimal BuAyMaasOdemesi { get; set; }
        public decimal BuAyAvans { get; set; }
        public decimal BuAyMasraf { get; set; }
        public decimal ToplamGider { get; set; }
        public decimal OncekiAyKarsilastirma { get; set; }
        public double DegisimYuzdesi { get; set; }
    }
}
