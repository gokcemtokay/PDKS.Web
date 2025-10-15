using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("parametreler")]
    public class Parametre
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("anahtar")]
        public string Anahtar { get; set; }
        public string Deger { get; set; }
        public string Aciklama { get; set; }
    }
}