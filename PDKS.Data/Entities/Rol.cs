using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("roller")]
    public class Rol
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("rol_adi")]
        public string RolAdi { get; set; } // Admin, IK, Yönetici, Personel

        [MaxLength(500)]
        [Column("aciklama")]
        public string Aciklama { get; set; }

        // Navigation Properties
        public virtual ICollection<Kullanici> Kullanicilar { get; set; }
    }
}
