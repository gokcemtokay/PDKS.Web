using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelSaglik")]
    public class PersonelSaglik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [StringLength(10)]
        public string? KanGrubu { get; set; } // A+, A-, B+, B-, AB+, AB-, 0+, 0-

        public int? Boy { get; set; } // cm

        public decimal? Kilo { get; set; } // kg

        [StringLength(1000)]
        public string? KronikHastaliklar { get; set; }

        [StringLength(1000)]
        public string? Alerjiler { get; set; }

        [StringLength(1000)]
        public string? SurekliKullanilanIlaclar { get; set; }

        public bool EngelDurumuVarMi { get; set; } = false;

        public int? EngelYuzdesi { get; set; } // %

        [StringLength(500)]
        public string? EngelAciklama { get; set; }

        [StringLength(500)]
        public string? SaglikRaporlari { get; set; } // Dosya yolları (JSON array olarak saklanabilir)

        public DateTime? SonPeriyodikMuayeneTarihi { get; set; }

        public DateTime? SonradakiPeriyodikMuayeneTarihi { get; set; }

        public DateTime? IsGuvenligiEgitimiTarihi { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
