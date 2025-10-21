using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Gorevler")]
    public class Gorev
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }

        public string? Aciklama { get; set; } // ✅ Column attribute kaldırıldı

        [Required]
        public int AtayanPersonelId { get; set; }

        public DateTime? BaslangicTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        [Required]
        [StringLength(50)]
        public string Oncelik { get; set; } = "Orta";

        [Required]
        [StringLength(50)]
        public string Durum { get; set; } = "Yeni";

        public int TamamlanmaYuzdesi { get; set; } = 0;

        public string? Etiketler { get; set; } // ✅ Column attribute kaldırıldı

        public string? Dosyalar { get; set; } // ✅ Column attribute kaldırıldı

        public int? UstGorevId { get; set; }

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("AtayanPersonelId")]
        public Personel Atayan { get; set; }

        [ForeignKey("UstGorevId")]
        public Gorev? UstGorev { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<Gorev> AltGorevler { get; set; }
        public ICollection<GorevAtama> GorevAtamalari { get; set; }
        public ICollection<GorevYorum> Yorumlar { get; set; }
    }
}