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

        [StringLength(100)]
        public string Departman { get; set; }

        [StringLength(100)]
        public string Gorev { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [StringLength(20)]
        public string Telefon { get; set; }

        public bool Durum { get; set; } = true;

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
