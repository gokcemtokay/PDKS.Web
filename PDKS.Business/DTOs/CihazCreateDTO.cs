using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class CihazCreateDTO
    {
        [Required(ErrorMessage = "Cihaz adı zorunludur")]
        [StringLength(100)]
        public string CihazAdi { get; set; }

        [StringLength(50)]
        public string IPAdres { get; set; }

        [StringLength(200)]
        public string Lokasyon { get; set; }

        public bool Durum { get; set; } = true;
    }
}
