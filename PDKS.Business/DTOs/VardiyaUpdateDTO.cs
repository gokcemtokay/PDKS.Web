using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class VardiyaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vardiya adı zorunludur")]
        [StringLength(100, ErrorMessage = "Vardiya adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Geçerli bir saat formatı giriniz (HH:mm)")]
        public string BaslangicSaati { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Geçerli bir saat formatı giriniz (HH:mm)")]
        public string BitisSaati { get; set; }

        public bool GeceVardiyasiMi { get; set; }

        public bool EsnekVardiyaMi { get; set; }

        [Range(0, 120, ErrorMessage = "Tolerans süresi 0-120 dakika arasında olmalıdır")]
        public int ToleransSuresiDakika { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Aciklama { get; set; }

        public bool Durum { get; set; }
    }
}