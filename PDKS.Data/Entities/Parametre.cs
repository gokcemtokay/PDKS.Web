using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Parametreler")]
    public class Parametre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required]
        [StringLength(100)]
        public string Deger { get; set; }

        [StringLength(50)]
        public string? Birim { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Kategori { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }
    }
}