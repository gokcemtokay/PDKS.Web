using System.ComponentModel.DataAnnotations;

namespace PDKS.Business.DTOs
{
    public class CihazCreateDTO
    {
        
        public int SirketId { get; set; }
public string CihazAdi { get; set; }

        [MaxLength(50, ErrorMessage = "Cihaz tipi en fazla 50 karakter olabilir")]
        public string CihazTipi { get; set; }  // ← YENİ ALAN

        [MaxLength(50, ErrorMessage = "IP adresi en fazla 50 karakter olabilir")]
        public string IPAdres { get; set; }

        public int? Port { get; set; }
        public string Lokasyon { get; set; }
        public bool Durum { get; set; }
    }
}