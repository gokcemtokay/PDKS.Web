// Data/Entities/Zimmet.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Zimmetler")]
    public class Zimmet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string DemirbasAdi { get; set; }

        [StringLength(50)]
        public string? DemirbasKodu { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        public DateTime TeslimTarihi { get; set; }

        public DateTime? IadeTarihi { get; set; }

        [Required]
        [StringLength(50)]
        public string Durum { get; set; } = "Personelde"; // Personelde, İade Edildi

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}