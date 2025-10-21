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
        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Sifre { get; set; }

        [StringLength(255)]
        public string? SifreHash { get; set; }

        public int? PersonelId { get; set; }

        [Required]
        public int RolId { get; set; }

        public bool Aktif { get; set; } = true;

        public DateTime? SonGirisTarihi { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? OlusturmaTarihi { get; set; } = DateTime.UtcNow;

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