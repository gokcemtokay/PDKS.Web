using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("DosyaTalepleri")]
    public class DosyaTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string DosyaAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string DosyaTipi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        [StringLength(50)]
        public string Durum { get; set; } = "Beklemede";

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? YuklenenDosya { get; set; }

        public DateTime? YuklemeTarihi { get; set; }

        public int? OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? OnayNotu { get; set; }

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public Kullanici? OnaylayanKullanici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<OnayAkisi> OnayAkislari { get; set; }
    }
}