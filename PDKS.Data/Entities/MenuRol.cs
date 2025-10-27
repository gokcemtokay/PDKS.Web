using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("MenuRoller")]
    public class MenuRol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public int RolId { get; set; }

        public bool Okuma { get; set; } = true; // Menüyü görebilir mi?

        // Navigation Properties
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }

        [ForeignKey("RolId")]
        public Rol Rol { get; set; }
    }
}
