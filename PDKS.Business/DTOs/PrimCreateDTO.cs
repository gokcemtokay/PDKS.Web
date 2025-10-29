using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PrimCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }

        [Required(ErrorMessage = "Dönem zorunludur")]
        public DateTime Donem { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur")]
        [Range(0.01, 999999, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Tutar { get; set; }

        [Required(ErrorMessage = "Prim tipi zorunludur")]
        [StringLength(200)]
        public string PrimTipi { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }
    }
}
