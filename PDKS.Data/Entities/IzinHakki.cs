// Data/Entities/IzinHakki.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("IzinHaklari")]
    public class IzinHakki
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int Yil { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal YillikIzinGun { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal KullanilanIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal KalanIzin { get; set; } // Computed column

        [Column(TypeName = "decimal(5,2)")]
        public decimal MazeretiIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal HastalikIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal UcretsizIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal BabalikIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal EvlilikIzin { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal OlumIzin { get; set; } = 0;

        [Required]
        public int SirketId { get; set; }

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}