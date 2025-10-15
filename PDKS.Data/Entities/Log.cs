using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("loglar")]
    public class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("kullanici_id")]
        public int? KullaniciId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("islem")]
        public string Islem { get; set; }

        [MaxLength(200)]
        [Column("modul")]
        public string Modul { get; set; }

        [Column("detay")]
        public string Detay { get; set; }

        [MaxLength(50)]
        [Column("ip_adres")]
        public string IpAdres { get; set; }

        [Column("tarih")]
        public DateTime Tarih { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("KullaniciId")]
        public virtual Kullanici Kullanici { get; set; }
    }
}
