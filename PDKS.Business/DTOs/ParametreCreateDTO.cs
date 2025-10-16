using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class ParametreCreateDTO
    {
        [Required(ErrorMessage = "Parametre adı zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Değer zorunludur")]
        [StringLength(100)]
        public string Deger { get; set; }

        [StringLength(50)]
        public string? Birim { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }

        [StringLength(50)]
        public string? Kategori { get; set; }
    }
}