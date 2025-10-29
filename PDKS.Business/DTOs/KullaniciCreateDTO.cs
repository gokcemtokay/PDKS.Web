using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class KullaniciCreateDTO
    {
        [Required]
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Sifre { get; set; }

        [Required]
        public int RolId { get; set; }

        public bool Aktif { get; set; } = true;

        // Eski View için geçici olarak eklendi, API'de kullanılmıyor.
        public string KullaniciAdi { get; set; }
    }
}