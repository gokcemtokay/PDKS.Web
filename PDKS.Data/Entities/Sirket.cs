using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("sirketler")]
    public class Sirket
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("unvan")]
        public string Unvan { get; set; }

        [MaxLength(200)]
        [Column("ticari_unvan")]
        public string? TicariUnvan { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("vergi_no")]
        public string VergiNo { get; set; }

        [MaxLength(100)]
        [Column("vergi_dairesi")]
        public string? VergiDairesi { get; set; }

        [MaxLength(20)]
        [Column("telefon")]
        public string? Telefon { get; set; }

        [MaxLength(100)]
        [Column("email")]
        public string? Email { get; set; }

        [MaxLength(500)]
        [Column("adres")]
        public string? Adres { get; set; }

        [MaxLength(50)]
        [Column("il")]
        public string? Il { get; set; }

        [MaxLength(50)]
        [Column("ilce")]
        public string? Ilce { get; set; }

        [MaxLength(10)]
        [Column("posta_kodu")]
        public string? PostaKodu { get; set; }

        [MaxLength(200)]
        [Column("website")]
        public string? Website { get; set; }

        [MaxLength(500)]
        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("kurulis_tarihi")]
        public DateTime? KurulusTarihi { get; set; }

        [Column("aktif")]
        public bool Aktif { get; set; } = true;

        [MaxLength(3)]
        [Column("para_birimi")]
        public string? ParaBirimi { get; set; } = "TRY";

        [MaxLength(1000)]
        [Column("notlar")]
        public string? Notlar { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        [Column("guncelleme_tarihi")]
        public DateTime? GuncellemeTarihi { get; set; }

        // Ana şirket mi? (holding yapısı için)
        [Column("ana_sirket")]
        public bool AnaSirket { get; set; } = false;

        // Bağlı olduğu ana şirket
        [Column("ana_sirket_id")]
        public int? AnaSirketId { get; set; }

        // Navigation Properties
        [ForeignKey("AnaSirketId")]
        public virtual Sirket AnaSirketNavigation { get; set; }

        public virtual ICollection<Sirket> BagliSirketler { get; set; } = new List<Sirket>();
        public virtual ICollection<Personel> Personeller { get; set; } = new List<Personel>();
        public virtual ICollection<Departman> Departmanlar { get; set; } = new List<Departman>();
        public virtual ICollection<KullaniciSirket> KullaniciSirketleri { get; set; } = new List<KullaniciSirket>(); // ⭐ YENİ EKLENEN SATIR
        public virtual ICollection<Cihaz> Cihazlar { get; set; } = new List<Cihaz>();
        public virtual ICollection<Vardiya> Vardiyalar { get; set; } = new List<Vardiya>();
    }
}