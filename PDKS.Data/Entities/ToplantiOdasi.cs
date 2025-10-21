using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("ToplantiOdalari")]
    public class ToplantiOdasi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string OdaAdi { get; set; }

        public int? Kapasite { get; set; }

        public string? Ozellikler { get; set; } // ✅ Düzeltildi - JSON

        public bool Aktif { get; set; } = true;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<ToplantiOdasiRezervasyon> Rezervasyonlar { get; set; }
    }
}