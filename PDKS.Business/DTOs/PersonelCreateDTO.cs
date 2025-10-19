// PDKS.Business/DTOs/PersonelCreateDTO.cs içine SirketId ekle
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PersonelCreateDTO
    {
        [Required(ErrorMessage = "Şirket seçimi zorunludur")]
        public int SirketId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Sicil No zorunludur")]
        [StringLength(20)]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 karakter olmalıdır")]
        public string TcKimlikNo { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string Telefon { get; set; }

        [StringLength(500)]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        public DateTime DogumTarihi { get; set; }

        [StringLength(10)]
        public string Cinsiyet { get; set; }

        [StringLength(50)]
        public string KanGrubu { get; set; }

        [Required(ErrorMessage = "Giriş tarihi zorunludur")]
        public DateTime GirisTarihi { get; set; }

        public DateTime? CikisTarihi { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maaş negatif olamaz")]
        public decimal? Maas { get; set; }

        [StringLength(100)]
        public string Unvan { get; set; }

        [StringLength(100)]
        public string Gorev { get; set; }
        public bool Durum { get; set; } = true;  

        public int? DepartmanId { get; set; }
        public int? VardiyaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Avans limiti negatif olamaz")]
        public decimal? AvansLimiti { get; set; }

        [StringLength(500)]
        public string Notlar { get; set; }
    }
}