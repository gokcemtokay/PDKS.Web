using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("personel_transfer_gecmisi")]
    public class PersonelTransferGecmisi
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Required]
        [Column("eski_sirket_id")]
        public int EskiSirketId { get; set; }

        [Required]
        [Column("yeni_sirket_id")]
        public int YeniSirketId { get; set; }

        [Column("eski_departman_id")]
        public int? EskiDepartmanId { get; set; }

        [Column("yeni_departman_id")]
        public int? YeniDepartmanId { get; set; }

        [MaxLength(100)]
        [Column("eski_unvan")]
        public string EskiUnvan { get; set; }

        [MaxLength(100)]
        [Column("yeni_unvan")]
        public string YeniUnvan { get; set; }

        [Column("eski_maas", TypeName = "decimal(18,2)")]
        public decimal? EskiMaas { get; set; }

        [Column("yeni_maas", TypeName = "decimal(18,2)")]
        public decimal? YeniMaas { get; set; }

        [Required]
        [Column("transfer_tarihi")]
        public DateTime TransferTarihi { get; set; }

        [MaxLength(20)]
        [Column("transfer_tipi")]
        public string TransferTipi { get; set; } // "Şirket İçi", "Şirketler Arası", "Terfi", "Görev Değişikliği"

        [MaxLength(1000)]
        [Column("sebep")]
        public string Sebep { get; set; }

        [MaxLength(1000)]
        [Column("notlar")]
        public string Notlar { get; set; }

        [Column("onaylayan_kullanici_id")]
        public int? OnaylayanKullaniciId { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("EskiSirketId")]
        public virtual Sirket EskiSirket { get; set; }

        [ForeignKey("YeniSirketId")]
        public virtual Sirket YeniSirket { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public virtual Kullanici OnaylayanKullanici { get; set; }
    }
}