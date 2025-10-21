using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("GorevTanimlari")]
    public class GorevTanimi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DepartmanId { get; set; }

        [Required]
        [StringLength(200)]
        public string GorevAdi { get; set; }

        [StringLength(2000)]
        public string? Aciklama { get; set; }

        public string? GerekliNitelikler { get; set; } // ✅ Column attribute kaldırıldı

        [StringLength(50)]
        public string? DeneyimSeviyesi { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaasAraligiMin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaasAraligiMax { get; set; }

        public bool Aktif { get; set; } = true;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("DepartmanId")]
        public Departman Departman { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}