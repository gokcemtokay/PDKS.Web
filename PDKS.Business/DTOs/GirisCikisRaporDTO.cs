namespace PDKS.Business.DTOs
{
    // Giriş-Çıkış Rapor DTO'ları
    public class GirisCikisRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public int CalismaSuresi { get; set; }
        public string CalismaSuresiText => $"{CalismaSuresi / 60}s {CalismaSuresi % 60}d";
        public string Durum { get; set; }
        public int GecKalmaSuresi { get; set; }
        public int ErkenCikisSuresi { get; set; }
        public int FazlaMesaiSuresi { get; set; }
    }
}
