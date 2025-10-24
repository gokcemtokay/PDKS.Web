using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelTerfi")]
    public class PersonelTerfi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime TerfiTarihi { get; set; }

        [StringLength(100)]
        public string? EskiPozisyon { get; set; }

        [Required]
        [StringLength(100)]
        public string YeniPozisyon { get; set; }

        [StringLength(100)]
        public string? EskiUnvan { get; set; }

        [Required]
        [StringLength(100)]
        public string YeniUnvan { get; set; }

        public int? EskiDepartmanId { get; set; }

        public int? YeniDepartmanId { get; set; }

        [StringLength(1000)]
        public string? TerfiNedeni { get; set; }

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

        [ForeignKey("EskiDepartmanId")]
        public virtual Departman? EskiDepartman { get; set; }

        [ForeignKey("YeniDepartmanId")]
        public virtual Departman? YeniDepartman { get; set; }
    }
}
