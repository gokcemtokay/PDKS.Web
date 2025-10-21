using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Sikayetler")]
    public class Sikayet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(300)]
        public string Baslik { get; set; }

        [Required]
        public string Aciklama { get; set; } // ✅ Düzeltildi

        [StringLength(100)]
        public string? Kategori { get; set; }

        [Required]
        [StringLength(50)]
        public string OncelikSeviyesi { get; set; } = "Orta";

        [Required]
        [StringLength(50)]
        public string Durum { get; set; } = "Yeni";

        public bool Anonim { get; set; } = false;

        public string? EkDosyalar { get; set; } // ✅ Düzeltildi

        public int? AtananKullaniciId { get; set; }

        public string? CozumAciklamasi { get; set; } // ✅ Düzeltildi

        public DateTime? CozumTarihi { get; set; }

        public DateTime SikayetTarihi { get; set; } = DateTime.UtcNow;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("AtananKullaniciId")]
        public Kullanici? AtananKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}