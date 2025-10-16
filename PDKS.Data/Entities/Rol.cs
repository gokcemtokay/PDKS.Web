using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Roller")]
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RolAdi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        // Navigation Properties
        public ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
    }
}