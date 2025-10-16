using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Avanslar")]
    public class Avans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tutar { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi

        [StringLength(50)]
        public string Durum { get; set; } = "Aktif"; // Aktif, Ödendi, İptal

        public int? OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? RedNedeni { get; set; }

        public bool OdendiMi { get; set; } = false;

        public DateTime? OdemeTarihi { get; set; }

        public int? TaksitSayisi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? KalanBorc { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public Kullanici? OnaylayanKullanici { get; set; }
    }
}