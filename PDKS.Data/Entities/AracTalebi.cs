// Data/Entities/AracTalebi.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("AracTalepleri")]
    public class AracTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        public int? AracId { get; set; } // Onaylandıktan sonra atanır

        [StringLength(100)]
        public string? GidisSehri { get; set; }

        [StringLength(100)]
        public string? DonusSehri { get; set; }

        [Required]
        public DateTime KalkisTarihi { get; set; }

        [Required]
        public TimeSpan KalkisSaati { get; set; }

        [Required]
        public DateTime DonusTarihi { get; set; }

        [Required]
        public TimeSpan DonusSaati { get; set; }

        public int YolcuSayisi { get; set; } = 1;

        [StringLength(500)]
        public string? Amac { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede";

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("AracId")]
        public Arac? Arac { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<OnayAkisi> OnayAkislari { get; set; }
    }
}