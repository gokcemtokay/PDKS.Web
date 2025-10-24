using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelAcilDurum")]
    public class PersonelAcilDurum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(20)]
        public string IletisimTipi { get; set; } // Birincil, İkincil

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required]
        [StringLength(50)]
        public string YakinlikDerecesi { get; set; } // Eş, Anne, Baba, Kardeş, Arkadaş

        [Required]
        [StringLength(15)]
        public string Telefon1 { get; set; }

        [StringLength(15)]
        public string? Telefon2 { get; set; }

        [StringLength(500)]
        public string? Adres { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
