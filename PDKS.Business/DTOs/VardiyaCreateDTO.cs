using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class VardiyaCreateDTO
    {
        [Required(ErrorMessage = "Vardiya adı zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan BaslangicSaati { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan BitisSaati { get; set; }

        public bool GeceVardiyasiMi { get; set; }
        public bool EsnekVardiyaMi { get; set; }

        [Range(0, 120, ErrorMessage = "Tolerans süresi 0-120 dakika arasında olmalıdır")]
        public int ToleransSuresiDakika { get; set; } = 15;

        [StringLength(500)]
        public string Aciklama { get; set; }

        public bool Durum { get; set; } = true;
    }
}
