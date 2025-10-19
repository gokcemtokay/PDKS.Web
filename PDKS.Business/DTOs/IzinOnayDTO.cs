using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class IzinOnayDTO
    {
        [Required]
        public int IzinId { get; set; }

        [Required]
        // Değerin "Onaylandı" veya "Reddedildi" olmasını bekliyoruz.
        public string OnayDurumu { get; set; }

        // Reddedildiyse bu alan zorunlu olabilir, bu kontrolü serviste yapacağız.
        public string RedNedeni { get; set; }
    }
}