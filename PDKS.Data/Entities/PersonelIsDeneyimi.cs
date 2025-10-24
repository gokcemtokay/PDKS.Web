using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    /// <summary>
    /// Personelin daha önceki iş deneyimleri
    /// </summary>
    [Table("PersonelIsDeneyimi")]
    public class PersonelIsDeneyimi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string SirketAdi { get; set; }

        [Required]
        [StringLength(100)]
        public string Pozisyon { get; set; }

        [StringLength(100)]
        public string? Departman { get; set; }

        [StringLength(50)]
        public string? Sektor { get; set; }

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        public DateTime? BitisTarihi { get; set; }

        public bool HalenCalisiyor { get; set; } = false;

        [StringLength(2000)]
        public string? IsAciklamasi { get; set; } // Görev ve sorumluluklar

        [StringLength(2000)]
        public string? Basarilar { get; set; } // İşte elde edilen başarılar

        [StringLength(500)]
        public string? AyrilmaNedeni { get; set; }

        [StringLength(100)]
        public string? ReferansKisi { get; set; }

        [StringLength(15)]
        public string? ReferansTelefon { get; set; }

        [StringLength(500)]
        public string? BelgeDosyaYolu { get; set; } // İş çıkış belgesi, referans mektubu vb.

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeTarihi { get; set; }



        public string? IsTanimi { get; set; }
        public string? ReferansKisiAdi { get; set; }
        public string? ReferansKisiTelefon { get; set; }
        public string? ReferansKisiEmail { get; set; }
        public bool SGKTescilliMi { get; set; }
        public string? Notlar { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

    }
}
