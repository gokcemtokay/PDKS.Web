using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("GorevAtamalari")]
    public class GorevAtama
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GorevId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        public DateTime AtamaTarihi { get; set; } = DateTime.UtcNow;

        public bool Tamamlandi { get; set; } = false;

        public DateTime? TamamlanmaTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("GorevId")]
        public Gorev Gorev { get; set; }

        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }
    }
}