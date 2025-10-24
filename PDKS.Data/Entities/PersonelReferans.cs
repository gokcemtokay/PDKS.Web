using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelReferans")]
    public class PersonelReferans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [StringLength(200)]
        public string? SirketKurum { get; set; }

        [StringLength(100)]
        public string? Pozisyon { get; set; }

        [Required]
        [StringLength(50)]
        public string Iliski { get; set; } // Eski Amir, Meslektaş, Müşteri, Akademik, Diğer

        [Required]
        [StringLength(15)]
        public string Telefon { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(1000)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
