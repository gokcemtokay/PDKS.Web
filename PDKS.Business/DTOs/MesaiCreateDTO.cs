using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class MesaiCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan BaslangicSaati { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan BitisSaati { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }
    }
}