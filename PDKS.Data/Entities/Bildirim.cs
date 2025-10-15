using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("bildirimler")]
    public class Bildirim
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("kullanici_id")]
        public int KullaniciId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("baslik")]
        public string Baslik { get; set; }

        [Required]
        [Column("mesaj")]
        public string Mesaj { get; set; }

        [MaxLength(50)]
        [Column("tip")]
        public string Tip { get; set; } // Info, Warning, Success, Error

        [Column("okundu")]
        public bool Okundu { get; set; } = false;

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }
    }
}
