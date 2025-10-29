using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class GirisCikisCreateDTO
    {
        [Required(ErrorMessage = "Personel seçimi zorunludur")]
        
        public int SirketId { get; set; }
public int PersonelId { get; set; }

        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }

        [StringLength(100)]
        public string Kaynak { get; set; }

        public int? CihazId { get; set; }
        public bool ElleGiris { get; set; } = false;

        [StringLength(500)]
        public string Not { get; set; }
    }
}
