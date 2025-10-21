using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("AracKullanimRaporlari")]
    public class AracKullanimRaporu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AracId { get; set; }

        [Required]
        public int AracTalebiId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int BaslangicKm { get; set; }

        [Required]
        public int BitisKm { get; set; }

        public int ToplamKm => BitisKm - BaslangicKm;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? YakitTutari { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DigerGiderler { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ToplamMaliyet => (YakitTutari ?? 0) + (DigerGiderler ?? 0);

        [StringLength(1000)]
        public string? Notlar { get; set; }

        [StringLength(500)]
        public string? FaturaDosyalari { get; set; } // JSON array

        public DateTime BaslangicTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("AracId")]
        public Arac Arac { get; set; }

        [ForeignKey("AracTalebiId")]
        public AracTalebi AracTalebi { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }
    }
}