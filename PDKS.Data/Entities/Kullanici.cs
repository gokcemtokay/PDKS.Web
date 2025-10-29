using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Kullanicilar")]
    public class Kullanici
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("kullaniciadi")]  // ✅ PostgreSQL kolon adı
        public string KullaniciAdi { get; set; }

        [Column("ad")]  // ✅ PostgreSQL kolon adı
        public string Ad { get; set; }

        [Column("soyad")]  // ✅ PostgreSQL kolon adı
        public string Soyad { get; set; }

        [Required]
        [StringLength(100)]
        [Column("email")]  // ✅ PostgreSQL kolon adı
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("sifre")]  // ✅ PostgreSQL kolon adı
        public string Sifre { get; set; }

        [StringLength(255)]
        [Column("sifrehash")]  // ✅ PostgreSQL kolon adı
        public string? SifreHash { get; set; }

        [Column("personelid")]  // ✅ PostgreSQL kolon adı
        public int? PersonelId { get; set; }

        [Required]
        [Column("rolid")]  // ✅ PostgreSQL kolon adı
        public int RolId { get; set; }

        [Column("aktif")]  // ✅ PostgreSQL kolon adı
        public bool Aktif { get; set; } = true;

        [Column("songiristarihi")]  // ✅ PostgreSQL kolon adı
        public DateTime? SonGirisTarihi { get; set; }

        [Column("kayittarihi")]  // ✅ PostgreSQL kolon adı
        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        [Column("olusturmatarihi")]  // ✅ PostgreSQL kolon adı
        public DateTime? OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        [Column("guncellemeTarihi")]  // ✅ PostgreSQL kolon adı
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel? Personel { get; set; }

        [ForeignKey("RolId")]
        public Rol Rol { get; set; }

        public ICollection<Log> Loglar { get; set; } = new List<Log>();
        public ICollection<Bildirim> Bildirimler { get; set; } = new List<Bildirim>();
        public virtual ICollection<KullaniciSirket> KullaniciSirketler { get; set; } = new List<KullaniciSirket>();
    }
}