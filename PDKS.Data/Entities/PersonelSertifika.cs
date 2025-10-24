using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("PersonelSertifika")]
    public class PersonelSertifika
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(200)]
        public string SertifikaAdi { get; set; }

        [Required]
        [StringLength(200)]
        public string VerenKurum { get; set; }

        [Required]
        public DateTime AlimTarihi { get; set; }

        public DateTime? GecerlilikTarihi { get; set; } // Opsiyonel - süreli sertifikalar için

        public bool SureliMi { get; set; } = false;

        [StringLength(100)]
        public string SertifikaNumarasi { get; set; }

        [StringLength(500)]
        public string? SertifikaDosyasi { get; set; } // Dosya yolu

        [Required]
        [StringLength(30)]
        public string Durum { get; set; } = "Geçerli"; // Geçerli, Süresi Dolmuş, Yenilenmeli

        public bool HatirlatmaGonderildiMi { get; set; } = false;

        public DateTime? HatirlatmaTarihi { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property
        [ForeignKey("PersonelId")]
        public virtual Personel Personel { get; set; }
    }
}
