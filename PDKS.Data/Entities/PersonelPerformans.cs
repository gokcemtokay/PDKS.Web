using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelPerformans")]
    public class PersonelPerformans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime DegerlendirmeTarihi { get; set; }

        [Required]
        [StringLength(50)]
        public string Donem { get; set; } // Aylık, 3 Aylık, 6 Aylık, Yıllık

        [Required]
        public int DegerlendiriciKullaniciId { get; set; }

        [Required]
        public decimal PerformansNotu { get; set; } // 1-100 arası veya 1-5 arası

        [StringLength(50)]
        public string? NotSkalasi { get; set; } // A-F veya 1-5 veya 1-100

        [StringLength(1000)]
        public string? Hedefler { get; set; }

        public decimal? HedefBasariOrani { get; set; } // %

        [StringLength(1000)]
        public string? GucluYonler { get; set; }

        [StringLength(1000)]
        public string? GelisimAlanlari { get; set; }

        [StringLength(2000)]
        public string? Yorumlar { get; set; }

        [StringLength(2000)]
        public string? AksiyonPlanlari { get; set; }

        [Required]
        [StringLength(30)]
        public string Durum { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Revize Gerekli

        public int? OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? EkDosyalar { get; set; } // JSON array olarak dosya yolları

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("DegerlendiriciKullaniciId")]
        public virtual Kullanici DegerlendiriciKullanici { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public virtual Kullanici? OnaylayanKullanici { get; set; }
    }
}
