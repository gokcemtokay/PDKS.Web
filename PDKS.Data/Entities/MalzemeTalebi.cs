using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("MalzemeTalepleri")]
    public class MalzemeTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string MalzemeAdi { get; set; }

        [Required]
        public int Miktar { get; set; }

        [StringLength(50)]
        public string? Birim { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede";

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? OnayTarihi { get; set; }

        public DateTime? TeslimTarihi { get; set; }

        public int? TeslimEdenKullaniciId { get; set; }

        [StringLength(500)]
        public string? TeslimNotu { get; set; }

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("TeslimEdenKullaniciId")]
        public Kullanici? TeslimEdenKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<OnayAkisi> OnayAkislari { get; set; }
    }
}