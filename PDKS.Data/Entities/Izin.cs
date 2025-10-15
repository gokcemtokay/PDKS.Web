using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("izinler")]
    public class Izin
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("izin_tipi")]
        public string IzinTipi { get; set; } // Yıllık, Raporlu, Ücretsiz, Doğum, Mazeret

        [Column("baslangic_tarihi")]
        public DateTime BaslangicTarihi { get; set; }

        [Column("bitis_tarihi")]
        public DateTime BitisTarihi { get; set; }

        [Column("gun_sayisi")]
        public int GunSayisi { get; set; }

        [MaxLength(1000)]
        [Column("aciklama")]
        public string Aciklama { get; set; }

        [MaxLength(50)]
        [Column("onay_durumu")]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi

        [Column("onaylayan_id")]
        public int? OnaylayanId { get; set; }

        [Column("onay_tarihi")]
        public DateTime? OnayTarihi { get; set; }

        [MaxLength(500)]
        [Column("red_nedeni")]
        public string RedNedeni { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("OnaylayanId")]
        public virtual Kullanici Onaylayan { get; set; }
    }
}
