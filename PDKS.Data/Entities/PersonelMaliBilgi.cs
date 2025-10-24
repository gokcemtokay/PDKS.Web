using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelMaliBilgi")]
    public class PersonelMaliBilgi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [StringLength(100)]
        public string? BankaAdi { get; set; }

        [StringLength(50)]
        public string? IBAN { get; set; }

        [StringLength(30)]
        public string? HesapTuru { get; set; } // Maaş, Bireysel, Ticari

        [StringLength(20)]
        public string? VergiNo { get; set; }

        [StringLength(100)]
        public string? VergiDairesi { get; set; }

        [StringLength(20)]
        public string? SGKNo { get; set; }

        public DateTime? SGKBaslangicTarihi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AsgariUcretMuafiyeti { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? GelirVergisiOrani { get; set; } // %

        public bool EmekliSandigi { get; set; } = false;

        [StringLength(50)]
        public string? OdemeYontemi { get; set; } // Banka Havalesi, Nakit, Çek

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
