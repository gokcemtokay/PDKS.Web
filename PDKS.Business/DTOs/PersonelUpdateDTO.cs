using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PersonelUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Sicil No zorunludur")]
        [StringLength(20, ErrorMessage = "Sicil No en fazla 20 karakter olabilir")]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 karakter olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik No sadece rakamlardan oluşmalıdır")]
        public string TcKimlikNo { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [StringLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [StringLength(15, ErrorMessage = "Telefon en fazla 15 karakter olabilir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string? Telefon { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string? Adres { get; set; }

        [Required(ErrorMessage = "Doğum Tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [StringLength(10, ErrorMessage = "Cinsiyet en fazla 10 karakter olabilir")]
        public string? Cinsiyet { get; set; }

        [StringLength(50, ErrorMessage = "Kan Grubu en fazla 50 karakter olabilir")]
        public string? KanGrubu { get; set; }

        [Required(ErrorMessage = "İşe Giriş Tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime GirisTarihi { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CikisTarihi { get; set; }

        [Range(0, 999999999, ErrorMessage = "Maaş 0 ile 999999999 arasında olmalıdır")]
        public decimal? Maas { get; set; }

        [StringLength(100, ErrorMessage = "Ünvan en fazla 100 karakter olabilir")]
        public string? Unvan { get; set; }

        [StringLength(100, ErrorMessage = "Görev en fazla 100 karakter olabilir")]
        public string? Gorev { get; set; }

        [Range(0, 999999999, ErrorMessage = "Avans Limiti 0 ile 999999999 arasında olmalıdır")]
        public decimal? AvansLimiti { get; set; }

        public bool Durum { get; set; }

        // Foreign Keys
        public int? DepartmanId { get; set; }

        // Display property - Geriye dönük uyumluluk için
        [StringLength(100)]
        public string? Departman { get; set; }

        public int? VardiyaId { get; set; }

        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        public string? Notlar { get; set; }
    }
}