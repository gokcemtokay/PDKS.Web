using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Hesaplama DTO
    public class PuantajHesaplaDTO
    {
        public int PersonelId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public bool YenidenHesapla { get; set; } = false;
    }
}
