using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelEgitimKayit")]
    public class PersonelEgitimKayit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string EgitimAdi { get; set; }

        [StringLength(200)]
        public string? EgitmenKurum { get; set; }

        [Required]
        public DateTime EgitimTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        public int? EgitimSuresiSaat { get; set; }

        [Required]
        [StringLength(30)]
        public string TamamlanmaDurumu { get; set; } = "Tamamlandı"; // Tamamlandı, Devam Ediyor, Vazgeçildi

        [Column(TypeName = "decimal(18,2)")]
        public decimal? EgitimMaliyeti { get; set; }

        [StringLength(500)]
        public string? EgitimSertifikasi { get; set; } // Dosya yolu

        public bool SertifikaAldiMi { get; set; } = false;

        [StringLength(50)]
        public string? EgitimTuru { get; set; } // Online, Yüz Yüze, Hibrit

        [StringLength(50)]
        public string? EgitimKategorisi { get; set; } // Teknik, Yönetsel, Kişisel Gelişim, Zorunlu

        [StringLength(1000)]
        public string? EgitimIcerigi { get; set; }

        public int? DegerlendirmePuani { get; set; } // 1-5 yıldız

        [StringLength(1000)]
        public string? PersonelGeribildirimi { get; set; }

        [StringLength(500)]
        public string? EkDosyalar { get; set; } // JSON array

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
