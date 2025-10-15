namespace PDKS.Business.DTOs
{
    // Maaş Zarfı DTO
    public class MaasZarfiDTO
    {
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Gorev { get; set; }
        public string Donem { get; set; }
        public decimal BrutMaas { get; set; }
        public decimal FazlaMesaiUcreti { get; set; }
        public decimal Primler { get; set; }
        public decimal ToplamBrut { get; set; }
        public decimal SGKKesintisi { get; set; }
        public decimal GelirVergisi { get; set; }
        public decimal AvansKesintisi { get; set; }
        public decimal NetMaas { get; set; }
        public string OdemeYontemi { get; set; }
        public DateTime OdemeTarihi { get; set; }
    }
}
