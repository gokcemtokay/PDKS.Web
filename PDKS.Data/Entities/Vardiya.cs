using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    public class Vardiya
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ad")]
        public string Ad { get; set; }

        [Column("baslangic_saati")]
        public TimeSpan BaslangicSaati { get; set; }

        [Column("bitis_saati")]
        public TimeSpan BitisSaati { get; set; }

        [Column("gece_vardiyasi_mi")]
        public bool GeceVardiyasiMi { get; set; }

        [Column("esnek_vardiya_mi")]
        public bool EsnekVardiyaMi { get; set; }

        [Column("tolerans_suresi_dakika")]
        public int ToleransSuresiDakika { get; set; } = 15; // Geç kalma toleransı

        [MaxLength(500)]
        [Column("aciklama")]
        public string Aciklama { get; set; }

        [Column("durum")]
        public bool Durum { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Personel> Personeller { get; set; }
    }
}
