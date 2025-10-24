using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelYetkinlik")]
    public class PersonelYetkinlik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string YetkinlikTipi { get; set; } // Teknik, Soft Skill

        [Required]
        [StringLength(100)]
        public string YetkinlikAdi { get; set; } // Python, Excel, SAP, İletişim, Liderlik, vb.

        [Required]
        [StringLength(30)]
        public string Seviye { get; set; } // Başlangıç, Orta, İleri, Uzman

        public int? SeviyePuani { get; set; } // 1-5 arası

        public DateTime? SonKullanimTarihi { get; set; }

        public bool SelfAssessment { get; set; } = true; // Kendi değerlendirmesi mi?

        public int? DegerlendiriciKullaniciId { get; set; }

        public DateTime? DegerlendirmeTarihi { get; set; }

        [StringLength(500)]
        public string? BelgelendirenDokumanlar { get; set; } // JSON array

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("DegerlendiriciKullaniciId")]
        public virtual Kullanici? DegerlendiriciKullanici { get; set; }
    }
}
