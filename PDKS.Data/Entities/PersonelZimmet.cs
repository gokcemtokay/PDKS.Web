using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelZimmet")]
    public class PersonelZimmet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string EsyaTipi { get; set; } // Laptop, Telefon, Tablet, Araç, Anahtar, Kart, Diğer

        [Required]
        [StringLength(200)]
        public string EsyaAdi { get; set; }

        [StringLength(100)]
        public string? MarkaModel { get; set; }

        [StringLength(100)]
        public string? SeriNumarasi { get; set; }

        [Required]
        public DateTime ZimmetTarihi { get; set; }

        public DateTime? IadeTarihi { get; set; }

        [Required]
        [StringLength(30)]
        public string ZimmetDurumu { get; set; } = "Aktif"; // Aktif, İade Edildi, Kayıp, Hasarlı

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Degeri { get; set; }

        [StringLength(500)]
        public string? ZimmetSozlesmesi { get; set; } // Dosya yolu

        [StringLength(500)]
        public string? ZimmetFotografi { get; set; } // Dosya yolu

        [StringLength(1000)]
        public string? Aciklama { get; set; }

        public int ZimmetVerenKullaniciId { get; set; }

        public int? IadeTeslimAlanKullaniciId { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("ZimmetVerenKullaniciId")]
        public virtual Kullanici ZimmetVerenKullanici { get; set; }

        [ForeignKey("IadeTeslimAlanKullaniciId")]
        public virtual Kullanici? IadeTeslimAlanKullanici { get; set; }
    }
}
