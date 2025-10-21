using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("GorevYorumlari")]
    public class GorevYorum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GorevId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public string Yorum { get; set; } // ✅ Column attribute kaldırıldı

        public string? Dosyalar { get; set; } // ✅ Column attribute kaldırıldı

        public DateTime YorumTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("GorevId")]
        public Gorev Gorev { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }
    }
}