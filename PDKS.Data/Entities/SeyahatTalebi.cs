using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("SeyahatTalepleri")]
    public class SeyahatTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string SeyahatTipi { get; set; }

        [Required]
        [StringLength(100)]
        public string GidisSehri { get; set; }

        [Required]
        [StringLength(100)]
        public string VarisSehri { get; set; }

        [StringLength(100)]
        public string? UlkeAdi { get; set; }

        [Required]
        public DateTime KalkisTarihi { get; set; }

        [Required]
        public DateTime DonusTarihi { get; set; }

        [Required]
        [StringLength(500)]
        public string Amac { get; set; }

        public bool KonaklamaGerekli { get; set; }

        [StringLength(50)]
        public string? UlasimTipi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BeklenenMaliyet { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede";

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? UcakBileti { get; set; }

        [StringLength(500)]
        public string? OtelRezervasyon { get; set; }

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<OnayAkisi> OnayAkislari { get; set; }
        public ICollection<SeyahatMasraf> Masraflar { get; set; }
    }
}