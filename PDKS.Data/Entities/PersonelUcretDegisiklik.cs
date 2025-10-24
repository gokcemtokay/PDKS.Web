using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelUcretDegisiklik")]
    public class PersonelUcretDegisiklik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime DegisiklikTarihi { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EskiMaas { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal YeniMaas { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DegisimYuzdesi { get; set; } // Hesaplanmış %

        [Column(TypeName = "decimal(18,2)")]
        public decimal FarkTutari { get; set; } // Hesaplanmış fark

        [Required]
        [StringLength(50)]
        public string DegisimNedeni { get; set; } // Terfi, Zam, Performans, Piyasa Ayarı, Yan Haklar, Diğer

        [StringLength(1000)]
        public string? Aciklama { get; set; }

        [Required]
        public int OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? EkDosyalar { get; set; } // JSON array

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public virtual Kullanici OnaylayanKullanici { get; set; }
    }
}
