using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class MesaiCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan BaslangicSaati { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan BitisSaati { get; set; }

        [StringLength(50)]
        public string MesaiTipi { get; set; } = "Normal"; // EKLEME

        [Required(ErrorMessage = "Onay Durumu zorunludur")]
        [StringLength(50)]
        public string OnayDurumu { get; set; } = "Beklemede"; // EKLEME

        [StringLength(500)]
        public string? Aciklama { get; set; }
    }
}