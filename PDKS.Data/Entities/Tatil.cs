using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("tatiller")]
    public class Tatil
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("ad")]
        public string Ad { get; set; }

        [Column("tarih")]
        public DateTime Tarih { get; set; }

        [Column("resmi_tatil")]
        public bool ResmiTatil { get; set; } = true;

        [MaxLength(500)]
        [Column("aciklama")]
        public string Aciklama { get; set; }
    }
}
