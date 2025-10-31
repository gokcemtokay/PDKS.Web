using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj İstatistik DTO
    public class PuantajIstatistikDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public int ToplamPersonelSayisi { get; set; }
        public int PuantajHesaplananSayisi { get; set; }
        public int PuantajOnaylananSayisi { get; set; } // TYPO DÜZELTİLDİ
        public int ToplamCalismaSuresi { get; set; }
        public int ToplamFazlaMesaiSuresi { get; set; }
        public int ToplamDevamsizlikGunSayisi { get; set; }
        public int ToplamIzinGunSayisi { get; set; }
    }
}
