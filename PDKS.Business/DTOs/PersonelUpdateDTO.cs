// PDKS.Business/DTOs/PersonelUpdateDTO.cs içine SirketId ekle
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PersonelUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Şirket seçimi zorunludur")]
        public int SirketId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Sicil No zorunludur")]
        [StringLength(20)]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11)]
        public string TcKimlikNo { get; set; }
        public string ProfilResmi { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        [Phone]
        public string Telefon { get; set; }

        [StringLength(500)]
        public string Adres { get; set; }

        [Required]
        public DateTime DogumTarihi { get; set; }

        [StringLength(10)]
        public string Cinsiyet { get; set; }

        [StringLength(50)]
        public string KanGrubu { get; set; }

        [Required]
        public DateTime GirisTarihi { get; set; }

        public DateTime? CikisTarihi { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Maas { get; set; }

        [StringLength(100)]
        public string Unvan { get; set; }

        [StringLength(100)]
        public string Gorev { get; set; }

        public int? DepartmanId { get; set; }
        public int? VardiyaId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? AvansLimiti { get; set; }

        public bool Durum { get; set; }

        [StringLength(500)]
        public string? Notlar { get; set; }
    }
}