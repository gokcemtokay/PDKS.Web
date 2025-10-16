using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Primler")]
    public class Prim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(100)]
        public string PrimAdi { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tutar { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        public int Ay { get; set; }

        public int Yil { get; set; }

        [StringLength(50)]
        public string Donem { get; set; } // "2024/01", "2024-Ocak" vb.

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        [StringLength(50)]
        public string PrimTipi { get; set; } = "Performans"; // Performans, Satış, Proje, Diğer

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi

        public int? OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        public bool OdendiMi { get; set; } = false;

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public Kullanici? OnaylayanKullanici { get; set; }
    }
}