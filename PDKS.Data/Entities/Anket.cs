using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Anketler")]
    public class Anket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string AnketBasligi { get; set; }

        public string? Aciklama { get; set; } // ✅ Column attribute kaldırıldı

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        [Required]
        public DateTime BitisTarihi { get; set; }

        public bool Anonim { get; set; } = false;

        public bool Aktif { get; set; } = true;

        public string? HedefKatilimcilar { get; set; } // ✅ Column attribute kaldırıldı

        [Required]
        public int OlusturanKullaniciId { get; set; }

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("OlusturanKullaniciId")]
        public Kullanici OlusturanKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<AnketSoru> Sorular { get; set; }
    }
}