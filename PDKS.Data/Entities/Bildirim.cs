using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Bildirimler")]
    public class Bildirim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KullaniciId { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }

        [StringLength(1000)]
        public string? Mesaj { get; set; }

        [StringLength(50)]
        public string? Tip { get; set; } // Bilgi, Uyarı, Hata, Başarı

        public bool Okundu { get; set; } = false;

        public DateTime? OkunmaTarihi { get; set; }

        [StringLength(500)]
        public string? Link { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // ✅ YENİ EKLENENLER - Referans için
        [StringLength(50)]
        public string? ReferansTip { get; set; } // Izin, Avans, Masraf, Arac, Seyahat, Gorev, Mazeret

        public int? ReferansId { get; set; } // İlgili kaydın ID'si

        // Navigation Properties
        [ForeignKey("KullaniciId")]
        public Kullanici Kullanici { get; set; }
    }
}