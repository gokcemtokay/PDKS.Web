using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class AvansCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur")]
        [Range(0.01, 999999, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Tutar { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }
    }
}
