using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class VardiyaCreateDTO
    {
        [Required(ErrorMessage = "Vardiya adı zorunludur")]
        [StringLength(100, ErrorMessage = "Vardiya adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Geçerli bir saat formatı giriniz (HH:mm)")]
        public string BaslangicSaati { get; set; } = string.Empty;
        

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Geçerli bir saat formatı giriniz (HH:mm)")]
        public string BitisSaati { get; set; } = string.Empty;

        public bool GeceVardiyasiMi { get; set; } = false;

        public bool EsnekVardiyaMi { get; set; } = false;

        [Range(0, 120, ErrorMessage = "Tolerans süresi 0-120 dakika arasında olmalıdır")]
        public int ToleransSuresiDakika { get; set; } = 15;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Aciklama { get; set; }

        public bool Durum { get; set; } = true;
    }
}