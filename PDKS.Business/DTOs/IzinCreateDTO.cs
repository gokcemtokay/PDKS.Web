using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class IzinCreateDTO
    {
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

        public int IzinGunSayisi { get; set; } // EKLEME: Hesaplama serviste yapılabilir

        [Required(ErrorMessage = "Onay Durumu zorunludur")]
        [StringLength(50, ErrorMessage = "Onay Durumu en fazla 50 karakter olabilir")]
        public string OnayDurumu { get; set; } = "Beklemede"; // EKLEME

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Aciklama { get; set; }
    }
}