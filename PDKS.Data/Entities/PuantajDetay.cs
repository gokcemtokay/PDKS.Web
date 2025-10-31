using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PuantajDetaylar")]
    public class PuantajDetay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PuantajId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        public int? GunTipi { get; set; } // 1: Hafta İçi, 2: Hafta Tatili, 3: Resmi Tatil

        // Giriş-Çıkış Bilgileri
        public DateTime? IlkGiris { get; set; }
        public DateTime? SonCikis { get; set; }
        public int? ToplamCalismaSuresi { get; set; } // dakika

        // Vardiya Bilgileri
        public int? VardiyaId { get; set; }
        public TimeSpan? VardiyaBaslangic { get; set; }
        public TimeSpan? VardiyaBitis { get; set; }
        public int? PlanlananCalismaSuresi { get; set; } // dakika

        // Durum Analizi
        public string CalismaDurumu { get; set; } // Normal, GecKalmis, ErkenCikmis, DevamsizExamiz, Izinli, HaftaTatili, ResmiTatil
        public int? GecKalmaSuresi { get; set; } // dakika
        public int? ErkenCikisSuresi { get; set; } // dakika
        public int? FazlaMesaiSuresi { get; set; } // dakika
        public int? EksikCalismaSuresi { get; set; } // dakika

        // İzin Bilgileri
        public bool IzinliMi { get; set; }
        public int? IzinId { get; set; }
        public string IzinTuru { get; set; }

        // Mola Bilgileri
        public int? ToplamMolaSuresi { get; set; } // dakika
        public int? MolaAdedi { get; set; }

        // Ek Bilgiler
        public bool ElleGirildiMi { get; set; }
        public bool DuzeltmeYapildiMi { get; set; }
        public string Notlar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PuantajId")]
        public virtual Puantaj Puantaj { get; set; }

        [ForeignKey("VardiyaId")]
        public virtual Vardiya Vardiya { get; set; }

        [ForeignKey("IzinId")]
        public virtual Izin Izin { get; set; }
    }
}
