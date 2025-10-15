namespace PDKS.Business.DTOs
{
    // Kart Unutanlar Rapor DTO
    public class KartUnutanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Not { get; set; }
    }
}
