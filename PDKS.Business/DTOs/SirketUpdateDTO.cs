// PDKS.Business/DTOs/SirketUpdateDTO.cs - LogoUrl ve AnaSirket ekle
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class SirketUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ünvan zorunludur")]
        [StringLength(200)]
        public string Unvan { get; set; }

        [StringLength(200)]
        public string TicariUnvan { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        [StringLength(10, MinimumLength = 10)]
        public string VergiNo { get; set; }

        [StringLength(100)]
        public string VergiDairesi { get; set; }

        [StringLength(20)]
        [Phone]
        public string Telefon { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(500)]
        public string Adres { get; set; }

        [StringLength(50)]
        public string Il { get; set; }

        [StringLength(50)]
        public string Ilce { get; set; }

        [StringLength(10)]
        public string PostaKodu { get; set; }

        [StringLength(200)]
        [Url]
        public string Website { get; set; }

        [StringLength(500)]
        public string LogoUrl { get; set; }         // ✅ Eklendi

        public DateTime? KurulusTarihi { get; set; }

        public bool Aktif { get; set; }

        [StringLength(3)]
        public string ParaBirimi { get; set; }

        [StringLength(1000)]
        public string Notlar { get; set; }

        public bool AnaSirket { get; set; }         // ✅ Eklendi

        public int? AnaSirketId { get; set; }
    }
}