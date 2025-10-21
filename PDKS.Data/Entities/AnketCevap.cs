using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("AnketCevaplari")]
    public class AnketCevap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SoruId { get; set; }

        public int? PersonelId { get; set; }

        public string? Cevap { get; set; } // ✅ Column attribute kaldırıldı

        public DateTime CevapTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SoruId")]
        public AnketSoru Soru { get; set; }

        [ForeignKey("PersonelId")]
        public Personel? Personel { get; set; }
    }
}