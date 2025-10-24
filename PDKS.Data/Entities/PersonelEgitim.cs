using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelEgitim")]
    public class PersonelEgitim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string EgitimSeviyesi { get; set; } // İlkokul, Ortaokul, Lise, Önlisans, Lisans, Y.Lisans, Doktora

        [Required]
        [StringLength(200)]
        public string OkulAdi { get; set; }

        [StringLength(200)]
        public string? Bolum { get; set; }

        public int BaslangicYili { get; set; }

        public int? BitisYili { get; set; }

        [Required]
        [StringLength(30)]
        public string MezuniyetDurumu { get; set; } // Mezun, Öğrenci, Terk

        public decimal? MezuniyetNotu { get; set; } // CGPA veya 100 üzerinden

        [StringLength(500)]
        public string? DiplomaDosyasi { get; set; } // Dosya yolu

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
