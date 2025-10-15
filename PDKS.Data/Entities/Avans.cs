using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("avanslar")]
    public class Avans
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Column("tutar")]
        public decimal Tutar { get; set; }

        [Column("talep_tarihi")]
        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

        [Column("odeme_tarihi")]
        public DateTime? OdemeTarihi { get; set; }

        [MaxLength(50)]
        [Column("durum")]
        public string Durum { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Ödendi, Reddedildi

        [MaxLength(500)]
        [Column("aciklama")]
        public string Aciklama { get; set; }

        [Column("onaylayan_id")]
        public int? OnaylayanId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("OnaylayanId")]
        public virtual Kullanici Onaylayan { get; set; }
    }
}
