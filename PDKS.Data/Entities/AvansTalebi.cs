// Data/Entities/AvansTalebi.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("AvansTalepleri")]
    public class AvansTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tutar { get; set; }

        [Required]
        [StringLength(500)]
        public string Sebep { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? OdemeSekli { get; set; } // Nakit, Havale, MaaştanKesinti

        public int? TaksitSayisi { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandi, Reddedildi, Odendi

        public DateTime? OdemeTarihi { get; set; }

        [StringLength(500)]
        public string? DekontDosyasi { get; set; }

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