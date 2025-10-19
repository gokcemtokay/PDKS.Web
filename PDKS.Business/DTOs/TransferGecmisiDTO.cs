// PDKS.Business/DTOs/TransferGecmisiDTO.cs
namespace PDKS.Business.DTOs
{
    public class TransferGecmisiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string EskiSirketAdi { get; set; }
        public string YeniSirketAdi { get; set; }
        public string EskiUnvan { get; set; }
        public string YeniUnvan { get; set; }
        public decimal? EskiMaas { get; set; }
        public decimal? YeniMaas { get; set; }
        public DateTime TransferTarihi { get; set; }
        public string TransferTipi { get; set; }
        public string Sebep { get; set; }
        public string Notlar { get; set; }
    }
}