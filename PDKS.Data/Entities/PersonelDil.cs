using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelDil")]
    public class PersonelDil
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string DilAdi { get; set; } // İngilizce, Almanca, Fransızca, vb.

        [Required]
        [StringLength(30)]
        public string Seviye { get; set; } // Başlangıç, Orta, İleri, Anadil

        public int OkumaSeviyesi { get; set; } = 1; // 1-5 yıldız

        public int YazmaSeviyesi { get; set; } = 1; // 1-5 yıldız

        public int KonusmaSeviyesi { get; set; } = 1; // 1-5 yıldız

        [StringLength(50)]
        public string? SertifikaTuru { get; set; } // TOEFL, IELTS, TELC, vb.

        public int? SertifikaPuani { get; set; }

        public DateTime? SertifikaTarihi { get; set; }

        [StringLength(500)]
        public string? SertifikaDosyasi { get; set; } // Dosya yolu

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
