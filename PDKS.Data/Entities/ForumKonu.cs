using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("ForumKonulari")]
    public class ForumKonu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ForumId { get; set; }

        [Required]
        public int OlusturanPersonelId { get; set; }

        [Required]
        [StringLength(300)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; } // ✅ Düzeltildi

        public bool Sabitlenmis { get; set; } = false;

        public bool Kilitli { get; set; } = false;

        public int GoruntulemeSayisi { get; set; } = 0;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("ForumId")]
        public Forum Forum { get; set; }

        [ForeignKey("OlusturanPersonelId")]
        public Personel OlusturanPersonel { get; set; }

        public ICollection<ForumMesaj> Mesajlar { get; set; }
    }
}