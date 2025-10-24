using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelAile")]
    public class PersonelAile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string YakinlikDerecesi { get; set; } // Eş, Anne, Baba, Çocuk, Kardeş

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [StringLength(11)]
        public string? TcKimlikNo { get; set; }

        public DateTime? DogumTarihi { get; set; }

        [StringLength(100)]
        public string? Meslek { get; set; }

        public bool CalisiyorMu { get; set; } = false;

        [StringLength(15)]
        public string? Telefon { get; set; }

        public bool OgrenciMi { get; set; } = false; // Çocuklar için

        public bool SGKBagimlisi { get; set; } = false; // SGK'dan bakmakla yükümlü mü?

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
