using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Puantajlar")]
    public class Puantaj
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SirketId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int Yil { get; set; }

        [Required]
        public int Ay { get; set; }

        // Çalışma İstatistikleri
        public int ToplamCalismaSaati { get; set; } // dakika
        public int NormalMesaiSaati { get; set; } // dakika
        public int FazlaMesaiSaati { get; set; } // dakika
        public int GeceMesaiSaati { get; set; } // dakika
        public int HaftaSonuMesaiSaati { get; set; } // dakika
        
        // Devamsızlık ve İzin
        public int ToplamCalisilanGun { get; set; }
        public int DevamsizlikGunu { get; set; }
        public int IzinGunu { get; set; }
        public int RaporluGun { get; set; }
        public int HaftaTatiliGunu { get; set; }
        public int ResmiTatilGunu { get; set; }
        
        // Geç Kalma ve Erken Çıkış
        public int GecKalmaGunu { get; set; }
        public int GecKalmaSuresi { get; set; } // dakika
        public int ErkenCikisGunu { get; set; }
        public int ErkenCikisSuresi { get; set; } // dakika
        
        // Eksik Çalışma
        public int EksikCalismaSaati { get; set; } // dakika
        
        // Durum ve Tarihler
        [StringLength(50)]
        public string Durum { get; set; } = "Taslak"; // Taslak, Onaylandı, Kapalı
        
        public DateTime? OnayTarihi { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        
        [StringLength(1000)]
        public string? Notlar { get; set; }
        
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("SirketId")]
        public virtual Sirket Sirket { get; set; }

        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public virtual Kullanici? OnaylayanKullanici { get; set; }

        public virtual ICollection<PuantajDetay> Detaylar { get; set; } = new List<PuantajDetay>();
    }
}
