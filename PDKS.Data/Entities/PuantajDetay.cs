using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PuantajDetaylar")]
    public class PuantajDetay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PuantajId { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        public int? VardiyaId { get; set; }
        public int? GirisCikisId { get; set; }

        // Planlanan Saatler (Vardiya bazlı)
        public TimeSpan? PlanlananGiris { get; set; }
        public TimeSpan? PlanlananCikis { get; set; }
        public int PlanlananSure { get; set; } // dakika

        // Gerçekleşen Saatler
        public DateTime? GerceklesenGiris { get; set; }
        public DateTime? GerceklesenCikis { get; set; }
        public int? GerceklesenSure { get; set; } // dakika

        // Hesaplamalar
        public int? NormalMesai { get; set; } // dakika
        public int? FazlaMesai { get; set; } // dakika
        public int? GeceMesai { get; set; } // dakika
        public int? GecKalmaSuresi { get; set; } // dakika
        public int? ErkenCikisSuresi { get; set; } // dakika
        public int? EksikSure { get; set; } // dakika

        // Durum Bilgileri
        [StringLength(50)]
        public string GunDurumu { get; set; } // Normal, Tatil, Izin, Rapor, Devamsiz, HaftaSonu, ResmiTatil
        
        [StringLength(100)]
        public string? IzinTuru { get; set; } // Yıllık, Mazeret, Ücretsiz, vb.
        
        public bool HaftaSonuMu { get; set; } = false;
        public bool ResmiTatilMi { get; set; } = false;
        public bool GecKaldiMi { get; set; } = false;
        public bool ErkenCiktiMi { get; set; } = false;
        public bool DevamsizMi { get; set; } = false;

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("PuantajId")]
        public virtual Puantaj Puantaj { get; set; }

        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }

        [ForeignKey("VardiyaId")]
        public virtual Vardiya? Vardiya { get; set; }

        [ForeignKey("GirisCikisId")]
        public virtual GirisCikis? GirisCikis { get; set; }
    }
}
