using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("personeller")]
    public class Personel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ad_soyad")]
        public string AdSoyad { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("sicil_no")]
        public string SicilNo { get; set; }

        [MaxLength(100)]
        [Column("departman")]
        public string Departman { get; set; }

        [MaxLength(100)]
        [Column("gorev")]
        public string Gorev { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Column("telefon")]
        public string Telefon { get; set; }

        [Column("durum")]
        public bool Durum { get; set; } = true; // Aktif/Pasif

        [Column("giris_tarihi")]
        public DateTime GirisTarihi { get; set; }

        [Column("cikis_tarihi")]
        public DateTime? CikisTarihi { get; set; }

        [Column("vardiya_id")]
        public int? VardiyaId { get; set; }

        [Column("maas")]
        public decimal Maas { get; set; }

        [Column("avans_limiti")]
        public decimal AvansLimiti { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        [Column("guncelleme_tarihi")]
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("VardiyaId")]
        public virtual Vardiya Vardiya { get; set; }

        public virtual ICollection<GirisCikis> GirisCikislar { get; set; }
        public virtual ICollection<Izin> Izinler { get; set; }
        public virtual ICollection<Avans> Avanslar { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
