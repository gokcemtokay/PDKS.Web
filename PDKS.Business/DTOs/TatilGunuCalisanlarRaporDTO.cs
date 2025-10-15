namespace PDKS.Business.DTOs
{
    // Tatil Günü Çalışanlar Rapor DTO
    public class TatilGunuCalisanlarRaporDTO
    {
        public string TatilAdi { get; set; }
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public int CalismaSuresi { get; set; }
        public string CalismaSuresiText => $"{CalismaSuresi / 60}s {CalismaSuresi % 60}d";
    }
}
