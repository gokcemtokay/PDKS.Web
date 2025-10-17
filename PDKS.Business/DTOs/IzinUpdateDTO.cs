using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class IzinUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "İzin tipi seçimi zorunludur")]
        [StringLength(50, ErrorMessage = "İzin tipi en fazla 50 karakter olabilir")]
        public string IzinTipi { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime BaslangicTarihi { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime BitisTarihi { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Aciklama { get; set; }
    }
}