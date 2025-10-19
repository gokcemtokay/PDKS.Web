using PDKS.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    public class GirisCikis
    {
        public int Id { get; set; }

        public int PersonelId { get; set; }
        public Personel Personel { get; set; }

        public int? CihazId { get; set; }
        public Cihaz Cihaz { get; set; }

        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }

        public string Durum { get; set; } // Örn: Normal, Geç Kalmış, Erken Çıkmış, Fazla Mesai
        public int? GecKalmaSuresi { get; set; } // dakika
        public int? ErkenCikisSuresi { get; set; } // dakika
        public int? FazlaMesaiSuresi { get; set; } // dakika

        public bool ElleGiris { get; set; } = false;
        public string Not { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; } // YENİ EKLENEN SATIR
    }
}