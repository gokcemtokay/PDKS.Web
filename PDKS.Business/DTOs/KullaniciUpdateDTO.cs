using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class KullaniciUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PersonelId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RolId { get; set; }

        [Required]
        public bool Aktif { get; set; }

        public string Sifre { get; set; }

        public string YeniSifre { get; set; }

        // Eski Edit.cshtml View'ının neden olduğu hatayı gidermek için eklendi.
        public string KullaniciAdi { get; set; }
    }
}