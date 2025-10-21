using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("SeyahatMasraflari")]
    public class SeyahatMasraf
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SeyahatId { get; set; }

        [Required]
        [StringLength(50)]
        public string MasrafTipi { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tutar { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(500)]
        public string? FaturaDosyasi { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? KdvOrani { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SeyahatId")]
        public SeyahatTalebi Seyahat { get; set; }
    }
}