using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("IslemYetkiler")]
    public class IslemYetki
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string IslemKodu { get; set; } // ornek: "personel.ekle", "izin.onayla"

        [Required]
        [StringLength(200)]
        public string IslemAdi { get; set; }

        [StringLength(100)]
        public string? ModulAdi { get; set; } // Personel, Izin, Avans vb.

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public bool Aktif { get; set; } = true;

        // Navigation Properties
        public ICollection<RolIslemYetki> RolIslemYetkiler { get; set; } = new List<RolIslemYetki>();
    }
}
