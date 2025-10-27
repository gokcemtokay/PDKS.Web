using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Menuler")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuAdi { get; set; }

        [StringLength(200)]
        public string? MenuKodu { get; set; } // Unique kod (ornek: "personel", "izin.liste")

        [StringLength(200)]
        public string? Url { get; set; }

        [StringLength(100)]
        public string? Icon { get; set; }

        public int? UstMenuId { get; set; }

        public int Sira { get; set; } = 0;

        public bool Aktif { get; set; } = true;

        // Navigation Properties
        [ForeignKey("UstMenuId")]
        public Menu? UstMenu { get; set; }

        public ICollection<Menu> AltMenuler { get; set; } = new List<Menu>();
        public ICollection<MenuRol> MenuRoller { get; set; } = new List<MenuRol>();
    }
}
