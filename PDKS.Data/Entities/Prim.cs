using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PDKS.Data.Entities
{
    [Table("primler")]
    public class Prim
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("personel_id")]
        public int PersonelId { get; set; }

        [Column("donem")]
        public DateTime Donem { get; set; } // Ay/Yıl

        [Column("tutar")]
        public decimal Tutar { get; set; }

        [MaxLength(200)]
        [Column("prim_tipi")]
        public string PrimTipi { get; set; } // Performans, Satış, Hedef vb.

        [MaxLength(500)]
        [Column("aciklama")]
        public string Aciklama { get; set; }

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
