using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("ToplantiOdasiRezervasyonlari")]
    public class ToplantiOdasiRezervasyon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OdaId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        [Required]
        public DateTime BitisTarihi { get; set; }

        [StringLength(200)]
        public string? Konu { get; set; }

        public string? Katilimcilar { get; set; } // ✅ Düzeltildi - JSON array

        [Required]
        [StringLength(50)]
        public string Durum { get; set; } = "Aktif";

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("OdaId")]
        public ToplantiOdasi Oda { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}