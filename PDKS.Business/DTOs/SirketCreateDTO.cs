using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class SirketCreateDTO
    {
        [Required(ErrorMessage = "Ünvan zorunludur")]
        [MaxLength(200)]
        public string Unvan { get; set; }

        [MaxLength(200)]
        public string TicariUnvan { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur")]
        [MaxLength(10)]
        public string VergiNo { get; set; }

        [MaxLength(100)]
        public string VergiDairesi { get; set; }

        [MaxLength(20)]
        public string Telefon { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Adres { get; set; }

        [MaxLength(50)]
        public string Il { get; set; }

        [MaxLength(50)]
        public string Ilce { get; set; }

        [MaxLength(10)]
        public string PostaKodu { get; set; }

        [MaxLength(200)]
        public string Website { get; set; }

        public DateTime? KurulusTarihi { get; set; }

        public bool Aktif { get; set; } = true;

        [MaxLength(3)]
        public string ParaBirimi { get; set; } = "TRY";

        [MaxLength(1000)]
        public string Notlar { get; set; }

        public bool AnaSirket { get; set; } = false;

        public int? AnaSirketId { get; set; }
    }
}
