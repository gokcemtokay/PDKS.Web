using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("MazeretBildirimleri")]
    public class MazeretBildirimi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        [StringLength(50)]
        public string MazeretTipi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public string? EkDosyalar { get; set; } // ✅ Düzeltildi

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede";

        public int? OnaylayiciPersonelId { get; set; }

        public DateTime? OnayTarihi { get; set; }

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayiciPersonelId")]
        public Personel? Onaylayici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}