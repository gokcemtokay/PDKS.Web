using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class AvansCreateDTO
    {
        public int PersonelId { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
    }
}
