// PDKS.Business/DTOs/KullaniciCreateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class KullaniciCreateDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir")]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Sifre { get; set; } = string.Empty;

        public int? PersonelId { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur")]
        public int RolId { get; set; }

        public bool Aktif { get; set; } = true;

        // ✅ ŞİRKET YETKİLERİ
        [Required(ErrorMessage = "En az bir şirket seçilmelidir")]
        [MinLength(1, ErrorMessage = "En az bir şirket seçilmelidir")]
        public List<int> YetkiliSirketIdler { get; set; } = new List<int>();

        [Required(ErrorMessage = "Varsayılan şirket seçilmelidir")]
        public int VarsayilanSirketId { get; set; }
    }
}
