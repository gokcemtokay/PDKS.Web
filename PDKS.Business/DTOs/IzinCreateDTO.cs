using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class IzinCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "İzin tipi zorunludur")]
        [StringLength(50)]
        public string IzinTipi { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
        public DateTime BaslangicTarihi { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
        public DateTime BitisTarihi { get; set; }

        [StringLength(1000)]
        public string Aciklama { get; set; }
    }
}
