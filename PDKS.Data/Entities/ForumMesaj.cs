using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("ForumMesajlari")]
    public class ForumMesaj
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KonuId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public string Mesaj { get; set; } // ✅ Düzeltildi

        public string? Dosyalar { get; set; } // ✅ Düzeltildi

        public int BegeniSayisi { get; set; } = 0;

        public DateTime MesajTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        public bool Silindi { get; set; } = false;

        // Navigation Properties
        [ForeignKey("KonuId")]
        public ForumKonu Konu { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }
    }
}