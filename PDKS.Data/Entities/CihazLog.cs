using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("cihaz_loglari")]
    public class CihazLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("cihaz_id")]
        public int CihazId { get; set; }

        [MaxLength(100)]
        [Column("islem")]
        public string Islem { get; set; }

        [Column("basarili")]
        public bool Basarili { get; set; }

        [Column("detay")]
        public string Detay { get; set; }

        [Column("tarih")]
        public DateTime Tarih { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("CihazId")]
        public virtual Cihaz Cihaz { get; set; }
    }
}
