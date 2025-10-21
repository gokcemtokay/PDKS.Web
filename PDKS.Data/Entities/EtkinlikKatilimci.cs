using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("EtkinlikKatilimcilari")]
    public class EtkinlikKatilimci
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EtkinlikId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string KatilimDurumu { get; set; } = "Beklemede";

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? KatilimTarihi { get; set; }

        [StringLength(500)]
        public string? Not { get; set; }

        // Navigation Properties
        [ForeignKey("EtkinlikId")]
        public Etkinlik Etkinlik { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }
    }
}