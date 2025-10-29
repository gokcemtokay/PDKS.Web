namespace PDKS.Business.DTOs
{
    // Maaş Bordrosu DTO
    public class MaasBordrosuDTO
    {
        
        public int SirketId { get; set; }
public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Donem { get; set; }
        public decimal BrutMaas { get; set; }
        public decimal FazlaMesaiUcreti { get; set; }
        public decimal Primler { get; set; }
        public decimal ToplamBrut { get; set; }
        public decimal SGKKesintisi { get; set; }
        public decimal GelirVergisi { get; set; }
        public decimal AvansKesintisi { get; set; }
        public decimal NetMaas { get; set; }
        public int CalismaGunSayisi { get; set; }
        public int FazlaMesaiSaati { get; set; }
    }
}
