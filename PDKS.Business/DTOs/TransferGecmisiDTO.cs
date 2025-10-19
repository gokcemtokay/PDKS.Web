using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    public class TransferGecmisiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string EskiSirketUnvan { get; set; }
        public string YeniSirketUnvan { get; set; }
        public string EskiDepartman { get; set; }
        public string YeniDepartman { get; set; }
        public string EskiUnvan { get; set; }
        public string YeniUnvan { get; set; }
        public decimal? EskiMaas { get; set; }
        public decimal? YeniMaas { get; set; }
        public DateTime TransferTarihi { get; set; }
        public string TransferTipi { get; set; }
        public string Sebep { get; set; }
        public string OnaylayanKullanici { get; set; }
    }
}
