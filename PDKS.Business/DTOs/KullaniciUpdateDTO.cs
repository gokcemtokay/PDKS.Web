using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class KullaniciUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50)]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(100)]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        // Şifre opsiyonel - sadece değiştirilmek istenirse gönderilir
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string? YeniSifre { get; set; }

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

        // Internal - Controller'da set edilir
        public string? Sifre { get; set; }
    }
}