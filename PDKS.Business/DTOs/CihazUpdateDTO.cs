using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class CihazUpdateDTO
    {
        public int SirketId { get; set; }

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Cihaz adı zorunludur")]
        [MaxLength(100, ErrorMessage = "Cihaz adı en fazla 100 karakter olabilir")]
        public string CihazAdi { get; set; }

        [MaxLength(50, ErrorMessage = "Cihaz tipi en fazla 50 karakter olabilir")]
        public string CihazTipi { get; set; }  // ← YENİ ALAN

        [MaxLength(50, ErrorMessage = "IP adresi en fazla 50 karakter olabilir")]
        public string IPAdres { get; set; }

        public int? Port { get; set; }  // ← YENİ ALAN

        [MaxLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
        public string Lokasyon { get; set; }

        public bool Durum { get; set; }
    }
}