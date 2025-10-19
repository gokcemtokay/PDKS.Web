// PDKS.Business/DTOs/SirketCreateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class SirketCreateDTO
    {
        [Required(ErrorMessage = "Ünvan zorunludur")]
        [StringLength(200, ErrorMessage = "Ünvan en fazla 200 karakter olabilir")]
        public string Unvan { get; set; }

        [StringLength(200, ErrorMessage = "Ticari ünvan en fazla 200 karakter olabilir")]
        public string TicariUnvan { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Vergi numarası 10 karakter olmalıdır")]
        public string VergiNo { get; set; }

        [StringLength(100, ErrorMessage = "Vergi dairesi en fazla 100 karakter olabilir")]
        public string VergiDairesi { get; set; }

        [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string Telefon { get; set; }

        [StringLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string Adres { get; set; }

        [StringLength(50, ErrorMessage = "İl en fazla 50 karakter olabilir")]
        public string Il { get; set; }

        [StringLength(50, ErrorMessage = "İlçe en fazla 50 karakter olabilir")]
        public string Ilce { get; set; }

        [StringLength(10, ErrorMessage = "Posta kodu en fazla 10 karakter olabilir")]
        public string PostaKodu { get; set; }

        [StringLength(200, ErrorMessage = "Website en fazla 200 karakter olabilir")]
        [Url(ErrorMessage = "Geçerli bir URL giriniz")]
        public string Website { get; set; }

        [StringLength(500, ErrorMessage = "Logo URL en fazla 500 karakter olabilir")]
        public string LogoUrl { get; set; }

        public DateTime? KurulusTarihi { get; set; }

        public bool Aktif { get; set; } = true;

        [StringLength(3, ErrorMessage = "Para birimi en fazla 3 karakter olabilir")]
        public string ParaBirimi { get; set; } = "TRY";

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        public string Notlar { get; set; }

        public bool AnaSirket { get; set; } = false;

        public int? AnaSirketId { get; set; }
    }
}