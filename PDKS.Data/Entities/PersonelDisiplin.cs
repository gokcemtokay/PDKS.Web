using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelDisiplin")]
    public class PersonelDisiplin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string DisiplinTuru { get; set; } // Uyarı, İhtar, Kınama, Ücret Kesintisi, İş Akdi Feshi

        [Required]
        public DateTime OlayTarihi { get; set; }

        [Required]
        [StringLength(2000)]
        public string Aciklama { get; set; }

        [StringLength(1000)]
        public string? UygulananCeza { get; set; }

        [Required]
        public int KararVerenYetkiliId { get; set; }

        [StringLength(500)]
        public string? IlgiliDokumanlar { get; set; } // JSON array olarak dosya yolları

        [Required]
        [StringLength(30)]
        public string Durum { get; set; } = "Aktif"; // Aktif, İptal, Süre Doldu

        public DateTime? IptalTarihi { get; set; }

        [StringLength(500)]
        public string? IptalNedeni { get; set; }

        public int? IptalEdenKullaniciId { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("KararVerenYetkiliId")]
        public virtual Kullanici KararVerenYetkili { get; set; }

        [ForeignKey("IptalEdenKullaniciId")]
        public virtual Kullanici? IptalEdenKullanici { get; set; }
    }
}
