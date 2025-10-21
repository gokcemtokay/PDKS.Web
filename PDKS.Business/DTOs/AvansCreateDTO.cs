using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class AvansCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur")]
        public decimal Tutar { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        public DateTime Tarih { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.UtcNow; // EKLEME

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // EKLEME

        [StringLength(50)]
        public string Durum { get; set; } = "Aktif"; // EKLEME

        public bool OdendiMi { get; set; } = false; // EKLEME
    }
}