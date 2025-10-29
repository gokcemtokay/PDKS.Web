using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class TatilUpdateDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }

        [Required(ErrorMessage = "Tatil adı zorunludur")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        public DateTime Tarih { get; set; }

        [StringLength(500)]
        public string? Aciklama { get; set; }
    }
}