using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Departmanlar")]
    public class Departman
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(50)]
        public string? Kod { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public int? UstDepartmanId { get; set; }

        public bool Durum { get; set; } = true;

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UstDepartmanId")]
        public Departman? UstDepartman { get; set; }

        public ICollection<Departman> AltDepartmanlar { get; set; } = new List<Departman>();
        public ICollection<Personel> Personeller { get; set; } = new List<Personel>();
    }
}