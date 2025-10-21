using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("AnketSorulari")]
    public class AnketSoru
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AnketId { get; set; }

        [Required]
        [StringLength(500)]
        public string SoruMetni { get; set; }

        [Required]
        [StringLength(50)]
        public string SoruTipi { get; set; }

        public string? Secenekler { get; set; } // ✅ Column attribute kaldırıldı

        public int Sira { get; set; }

        public bool Zorunlu { get; set; } = false;

        // Navigation Properties
        [ForeignKey("AnketId")]
        public Anket Anket { get; set; }

        public ICollection<AnketCevap> Cevaplar { get; set; }
    }
}