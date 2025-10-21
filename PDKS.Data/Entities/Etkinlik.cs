using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Etkinlikler")]
    public class Etkinlik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string EtkinlikAdi { get; set; }

        public string? Aciklama { get; set; } // ✅ Düzeltildi

        [Required]
        [StringLength(50)]
        public string EtkinlikTipi { get; set; }

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        [Required]
        public DateTime BitisTarihi { get; set; }

        [StringLength(200)]
        public string? Konum { get; set; }

        public int? KontenjanSayisi { get; set; }

        public bool KatilimZorunlu { get; set; } = false;

        public string? HedefKatilimcilar { get; set; } // ✅ Düzeltildi

        [StringLength(500)]
        public string? KapakResmi { get; set; }

        [Required]
        public int DuzenleyenKullaniciId { get; set; }

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("DuzenleyenKullaniciId")]
        public Kullanici DuzenleyenKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<EtkinlikKatilimci> Katilimcilar { get; set; }
    }
}