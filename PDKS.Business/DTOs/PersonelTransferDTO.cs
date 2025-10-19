using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class PersonelTransferDTO
    {
        [Required]
        public int PersonelId { get; set; }

        [Required]
        public int YeniSirketId { get; set; }

        public int? YeniDepartmanId { get; set; }

        [MaxLength(100)]
        public string YeniUnvan { get; set; }

        public decimal? YeniMaas { get; set; }

        [Required]
        public DateTime TransferTarihi { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string TransferTipi { get; set; } = "Şirketler Arası";

        [MaxLength(1000)]
        public string Sebep { get; set; }

        [MaxLength(1000)]
        public string Notlar { get; set; }
    }
}
