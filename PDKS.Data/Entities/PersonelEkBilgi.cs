using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelEkBilgi")]
    public class PersonelEkBilgi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [StringLength(30)]
        public string? MedeniDurum { get; set; } // Bekar, Evli, Boşanmış, Dul

        [StringLength(30)]
        public string? AskerlikDurumu { get; set; } // Yapıldı, Muaf, Tecilli, Yapılmadı

        public DateTime? AskerlikBaslangicTarihi { get; set; }

        public DateTime? AskerlikBitisTarihi { get; set; }

        [StringLength(100)]
        public string? AskerlikYeri { get; set; }

        [StringLength(50)]
        public string? AskerlikRutbesi { get; set; }

        [StringLength(100)]
        public string? EhliyetSiniflari { get; set; } // B, C, D, E (Virgülle ayrılmış)

        public DateTime? EhliyetAlisTarihi { get; set; }

        public DateTime? EhliyetGecerlilikTarihi { get; set; }

        [StringLength(50)]
        public string? Uyruk { get; set; } // T.C., Çifte Uyruklu, Yabancı

        [StringLength(50)]
        public string? IkametIli { get; set; }

        [StringLength(50)]
        public string? IkametIlce { get; set; }

        [StringLength(500)]
        public string? IkametAdresi { get; set; }

        [StringLength(500)]
        public string? DogumYeri { get; set; }

        [StringLength(100)]
        public string? AnneAdi { get; set; }

        [StringLength(100)]
        public string? BabaAdi { get; set; }

        public int? CocukSayisi { get; set; }

        [StringLength(100)]
        public string? HobiIlgiAlanlari { get; set; }

        [StringLength(50)]
        public string? SosyalGuvence { get; set; } // SGK, Bağkur, Emekli Sandığı, Yok

        public bool SigortaliMi { get; set; } = false;

        [StringLength(100)]
        public string? SigortaSirketi { get; set; }

        [StringLength(50)]
        public string? SigortaPoliceNo { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
