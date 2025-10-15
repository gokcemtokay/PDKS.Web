using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("kullanicilar")]
    public class Kullanici
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("sifre_hash")]
        public string SifreHash { get; set; }

        [Required]
        [Column("rol_id")]
        public int RolId { get; set; }

        [Column("aktif")]
        public bool Aktif { get; set; } = true;

        [Column("son_giris_tarihi")]
        public DateTime? SonGirisTarihi { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }

        public virtual ICollection<Log> Loglar { get; set; }
    }
}
