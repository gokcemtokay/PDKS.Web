using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Duyurular")]
    public class Duyuru
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; } // ✅ Column attribute kaldırıldı

        [StringLength(50)]
        public string? Tip { get; set; }

        public DateTime BaslangicTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        public bool Aktif { get; set; } = true;

        public bool AnaSayfadaGoster { get; set; } = true;

        public string? HedefDepartmanlar { get; set; } // ✅ Column attribute kaldırıldı

        public string? EkDosyalar { get; set; } // ✅ Column attribute kaldırıldı

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
    }
}