// Data/Entities/Arac.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Araclar")]
    public class Arac
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Plaka { get; set; }

        [StringLength(100)]
        public string? Marka { get; set; }

        [StringLength(100)]
        public string? Model { get; set; }

        public int? Yil { get; set; }

        [StringLength(50)]
        public string? Renk { get; set; }

        [StringLength(50)]
        public string? YakitTipi { get; set; }

        public int GuncelKm { get; set; } = 0;

        public DateTime? SonMuayeneTarihi { get; set; }

        public DateTime? KaskoTarihi { get; set; }

        public DateTime? SigortaTarihi { get; set; }

        public bool Aktif { get; set; } = true;

        public int? TahsisliPersonelId { get; set; } // Tahsisli araçlar için

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("TahsisliPersonelId")]
        public Personel? TahsisliPersonel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<AracTalebi> AracTalepleri { get; set; }
    }
}