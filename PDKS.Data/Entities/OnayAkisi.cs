// Data/Entities/OnayAkisi.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("OnayAkislari")]
    public class OnayAkisi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayTipi { get; set; } // Izin, Avans, Masraf, Arac, Seyahat, Gorev, Mazeret, DosyaTalebi

        [Required]
        public int ReferansId { get; set; } // İlgili talebin ID'si

        [Required]
        public int Sira { get; set; } // Onay sırası (1, 2, 3...)

        [Required]
        public int OnaylayiciPersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandi, Reddedildi

        public DateTime? OnayTarihi { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [Required]
        public int SirketId { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("OnaylayiciPersonelId")]
        public Personel Onaylayici { get; set; }

        [ForeignKey("SirketId")]
        public Sirket Sirket { get; set; }
    }
}