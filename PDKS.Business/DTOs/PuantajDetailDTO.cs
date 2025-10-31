using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Detay DTO
    public class PuantajDetailDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string VardiyaAdi { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }

        // Çalışma İstatistikleri
        public int ToplamCalismaSuresi { get; set; }
        public int NormalMesaiSuresi { get; set; }
        public int FazlaMesaiSuresi { get; set; }
        public int GeceMesaiSuresi { get; set; }
        public int HaftaTatiliCalismaSuresi { get; set; }
        public int ResmiTatilCalismaSuresi { get; set; }

        // Gün Sayıları
        public int ToplamCalisilanGun { get; set; }
        public int GecKalmaGunSayisi { get; set; }
        public int ErkenCikisGunSayisi { get; set; }
        public int DevamsizlikGunSayisi { get; set; }
        public int IzinGunSayisi { get; set; }
        public int HastaTatiliGunSayisi { get; set; }
        public int MazeretliIzinGunSayisi { get; set; }
        public int UcretsizIzinGunSayisi { get; set; }
        public int HaftaTatiliGunSayisi { get; set; }
        public int ResmiTatilGunSayisi { get; set; }

        // Süre Detayları
        public int ToplamGecKalmaSuresi { get; set; }
        public int ToplamErkenCikisSuresi { get; set; }
        public int ToplamEksikCalismaSuresi { get; set; }

        // Durum
        public string Durum { get; set; }
        public bool Onaylandi { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public string OnaylayanAdSoyad { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string Notlar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public List<PuantajDetayItemDTO> Detaylar { get; set; }
    }
}
