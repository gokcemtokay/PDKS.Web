namespace PDKS.Business.DTOs
{
    // Elle Giriş Rapor DTO
    public class ElleGirisRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Not { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}
