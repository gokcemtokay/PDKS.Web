using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("RolIslemYetkiler")]
    public class RolIslemYetki
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public int IslemYetkiId { get; set; }

        public bool Izinli { get; set; } = true;

        // Navigation Properties
        [ForeignKey("RolId")]
        public Rol Rol { get; set; }

        [ForeignKey("IslemYetkiId")]
        public IslemYetki IslemYetki { get; set; }
    }
}
