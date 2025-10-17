using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDKS.Data.Entities
{
    [Table("Izinler")]
    public class Izin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [StringLength(50)]
        public string IzinTipi { get; set; } // Yıllık, Mazeret, Hastalık, Ücretsiz, Evlilik, Vefat, Doğum

        [Required]
        public DateTime BaslangicTarihi { get; set; }

        [Required]
        public DateTime BitisTarihi { get; set; }

        public int IzinGunSayisi { get; set; }  

        [Required]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Onaylandı, Reddedildi

        public int? OnaylayanKullaniciId { get; set; }  

        public DateTime? OnayTarihi { get; set; }

        [StringLength(1000)]
        public string? Aciklama { get; set; }

        [StringLength(500)]
        public string? RedNedeni { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;  
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;  

        // Navigation Properties
        [ForeignKey("PersonelId")]
        public Personel Personel { get; set; }

        [ForeignKey("OnaylayanKullaniciId")]
        public Kullanici? OnaylayanKullanici { get; set; }

    }
}