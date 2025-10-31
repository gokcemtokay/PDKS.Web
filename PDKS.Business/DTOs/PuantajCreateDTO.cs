using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Oluşturma DTO
    public class PuantajCreateDTO
    {
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public bool YenidenHesapla { get; set; }
        public string Notlar { get; set; }
    }
}
