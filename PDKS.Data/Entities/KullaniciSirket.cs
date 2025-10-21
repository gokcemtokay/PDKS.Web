// PDKS.Data/Entities/KullaniciSirket.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("kullanici_sirketler")]
    public class KullaniciSirket
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("kullanici_id")]
        public int KullaniciId { get; set; }

        [Required]
        [Column("sirket_id")]
        public int SirketId { get; set; }

        [Column("varsayilan")]
        public bool Varsayilan { get; set; } = false;

        [Column("aktif")]
        public bool Aktif { get; set; } = true;

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }

        [ForeignKey("SirketId")]
        public virtual Sirket Sirket { get; set; }
    }
}