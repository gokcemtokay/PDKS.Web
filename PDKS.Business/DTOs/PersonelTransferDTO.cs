// PDKS.Business/DTOs/PersonelTransferDTO.cs - TAMAMI
using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class PersonelTransferDTO
    {
        [Required]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Eski şirket bilgisi zorunludur")]
        public int EskiSirketId { get; set; }  // ✅ Eklendi

        [Required(ErrorMessage = "Yeni şirket zorunludur")]
        public int YeniSirketId { get; set; }

        public int? YeniDepartmanId { get; set; }

        [StringLength(100, ErrorMessage = "Ünvan en fazla 100 karakter olabilir")]
        public string YeniUnvan { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maaş negatif olamaz")]
        public decimal? YeniMaas { get; set; }

        [Required(ErrorMessage = "Transfer tarihi zorunludur")]
        public DateTime TransferTarihi { get; set; }

        [Required(ErrorMessage = "Transfer tipi zorunludur")]
        [StringLength(20, ErrorMessage = "Transfer tipi en fazla 20 karakter olabilir")]
        public string TransferTipi { get; set; } = "Şirketler Arası"; // "Şirket İçi", "Şirketler Arası", "Terfi", "Görev Değişikliği"

        [StringLength(1000, ErrorMessage = "Sebep en fazla 1000 karakter olabilir")]
        public string Sebep { get; set; }

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        public string Notlar { get; set; }
    }
}