using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class KullaniciUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır")]
        public string KullaniciAdi { get; set; }

        public int? PersonelId { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur")]
        public int RolId { get; set; }

        public bool Aktif { get; set; }
    }
}