using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("MasrafTalepleri")]
    public class MasrafTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string MasrafTipi { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tutar { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        [StringLength(500)]
        public string Aciklama { get; set; }

        public string? Faturalar { get; set; } // ✅ Düzeltildi

        [Column(TypeName = "decimal(5,2)")]
        public decimal? KdvOrani { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KdvTutari { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede";

        public DateTime? OdemeTarihi { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<OnayAkisi> OnayAkislari { get; set; }
    }
}