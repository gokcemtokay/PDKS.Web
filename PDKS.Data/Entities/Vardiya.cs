using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Vardiyalar")]
    public class Vardiya
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SirketId { get; set; }
        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required]
        public TimeSpan BaslangicSaati { get; set; }

        [Required]
        public TimeSpan BitisSaati { get; set; }

        public bool GeceVardiyasiMi { get; set; } = false;

        public bool EsnekVardiyaMi { get; set; } = false;

        public int ToleransSuresiDakika { get; set; } = 0;

        [StringLength(500)]
        public string? Aciklama { get; set; }

        public bool Durum { get; set; } = true;
        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
        // Navigation Properties
        public ICollection<Personel> Personeller { get; set; } = new List<Personel>();
    }
}