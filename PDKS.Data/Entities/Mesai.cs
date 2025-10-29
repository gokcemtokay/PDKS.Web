using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Mesailer")]
    public class Mesai
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        public TimeSpan BitisSaati { get; set; }

        public decimal ToplamSaat { get; set; }

        public decimal FazlaMesaiSaati { get; set; }

        public decimal HaftaSonuMesaiSaati { get; set; }

        public decimal ResmiTatilMesaiSaati { get; set; }

        [StringLength(50)]
        public string MesaiTipi { get; set; } = "Normal"; // Normal, FazlaMesai, HaftaSonu, ResmiTatil

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi

        public int? OnaylayanKullaniciId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(500)]
        public string? RedNedeni { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public Kullanici? OnaylayanKullanici { get; set; }
    }
}