using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Oneriler")]
    public class Oneri
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
        public string Durum { get; set; } = "Beklemede";

        public bool Anonim { get; set; } = false;

        public string? EkDosyalar { get; set; } // ✅ Düzeltildi

        public int? DegerlendirmePuani { get; set; }

        [StringLength(1000)]
        public string? DegerlendirmeNotu { get; set; }

        public int? DegerlendirenKullaniciId { get; set; }

        public DateTime? DegerlendirmeTarihi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OdulMiktari { get; set; }

        public DateTime OneriTarihi { get; set; } = DateTime.UtcNow;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("DegerlendirenKullaniciId")]
        public Kullanici? DegerlendirenKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}