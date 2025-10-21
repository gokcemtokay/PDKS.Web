using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Forumlar")]
    public class Forum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ForumAdi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Ikon { get; set; }

        public int Sira { get; set; } = 0;

        public bool Aktif { get; set; } = true;

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }

        public ICollection<ForumKonu> Konular { get; set; }
    }
}