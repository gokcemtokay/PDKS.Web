// PDKS.Data/Entities/Departman.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Departmanlar")]
    public class Departman
    {
        [Key]
        public int Id { get; set; }

        // ⭐ YENİ: Şirket bağlantısı
        [Required]
        public int SirketId { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [StringLength(50)]
        public string? Kod { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public int? UstDepartmanId { get; set; }

        public bool Durum { get; set; } = true;

        // ⭐ YENİ: Aktif property eklendi
        public bool Aktif { get; set; } = true;

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        // ⭐ YENİ
        public DateTime? OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        // ⭐ YENİ
        [ForeignKey("SirketId")]
        public virtual Sirket Sirket { get; set; }

        [ForeignKey("UstDepartmanId")]
        public Departman? UstDepartman { get; set; }

        public ICollection<Departman> AltDepartmanlar { get; set; } = new List<Departman>();
        public ICollection<Personel> Personeller { get; set; } = new List<Personel>();
    }
}