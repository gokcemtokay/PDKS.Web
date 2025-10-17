using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("giris_cikislar")]
    public class GirisCikis
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Column("giris_zamani")]
        public DateTime? GirisZamani { get; set; }

        [Column("cikis_zamani")]
        public DateTime? CikisZamani { get; set; }

        [MaxLength(100)]
        [Column("kaynak")]
        public string Kaynak { get; set; } // Cihaz adı

        [Column("cihaz_id")]
        public int? CihazId { get; set; }

        [Column("fazla_mesai_suresi")]
        public int? FazlaMesaiSuresi { get; set; } // Dakika cinsinden

        [Column("gec_kalma_suresi")]
        public int? GecKalmaSuresi { get; set; } // Dakika cinsinden

        [Column("erken_cikis_suresi")]
        public int? ErkenCikisSuresi { get; set; } // Dakika cinsinden

        [MaxLength(50)]
        [Column("durum")]
        public string Durum { get; set; } = "Normal";// Normal, Geç Kalmış, Erken Çıkmış, vb.

        [Column("elle_giris")]
        public bool ElleGiris { get; set; } = false;

        [MaxLength(500)]
        [Column("not")]
        public string Not { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(PersonelId))]
        public virtual Personel Personel { get; set; }

        [ForeignKey(nameof(CihazId))]
        public virtual Cihaz Cihaz { get; set; }
    }
}
