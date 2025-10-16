using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PersonelCreateDTO
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Sicil No zorunludur")]
        [StringLength(20)]
        public string SicilNo { get; set; }
        public string TcKimlikNo { get; set; }
        public int? DepartmanId { get; set; }
        public string? DepartmanAdi { get; set; }
        public string Departman { get; set; }  // Geriye dönük uyumluluk için

        [StringLength(100)]
        public string Gorev { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [StringLength(20)]
        public string Telefon { get; set; }
        public string? Adres { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string? Cinsiyet { get; set; }
        public bool Durum { get; set; } = true;
        public string? KanGrubu { get; set; }
        public string? Unvan { get; set; }
        public string? Notlar { get; set; }

        [Required(ErrorMessage = "Giriş tarihi zorunludur")]
        public DateTime GirisTarihi { get; set; } = DateTime.Today;

        public DateTime? CikisTarihi { get; set; }

        public int? VardiyaId { get; set; }

        [Range(0, 999999, ErrorMessage = "Geçerli bir maaş tutarı giriniz")]
        public decimal Maas { get; set; }

        [Range(0, 999999, ErrorMessage = "Geçerli bir avans limiti giriniz")]
        public decimal AvansLimiti { get; set; }
    }
}
