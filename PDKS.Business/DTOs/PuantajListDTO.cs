using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class PuantajListDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public string Donem => $"{Yil}/{Ay:00}";
        public int ToplamCalisilanGun { get; set; }
        public decimal ToplamCalismaSaati => ToplamCalismaSuresi / 60.0m;
        public int ToplamCalismaSuresi { get; set; }
        public decimal FazlaMesaiSaati => FazlaMesaiSuresi / 60.0m;
        public int FazlaMesaiSuresi { get; set; }
        public int DevamsizlikGunSayisi { get; set; }
        public int IzinGunSayisi { get; set; }
        public string Durum { get; set; }
        public bool Onaylandi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}
