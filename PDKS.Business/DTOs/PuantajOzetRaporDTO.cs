using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Özet Rapor DTO
    public class PuantajOzetRaporDTO
    {
        public int ToplamPersonelSayisi { get; set; }
        public decimal ToplamCalismaSaati { get; set; }
        public decimal ToplamFazlaMesai { get; set; }
        public int ToplamDevamsizlik { get; set; }
        public int ToplamIzin { get; set; }
    }
}
