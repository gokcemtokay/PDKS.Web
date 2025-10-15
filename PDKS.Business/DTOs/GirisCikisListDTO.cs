namespace PDKS.Business.DTOs
{
    // GirisCikis DTOs
    public class GirisCikisListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Kaynak { get; set; }
        public int? FazlaMesaiSuresi { get; set; }
        public int? GecKalmaSuresi { get; set; }
        public int? ErkenCikisSuresi { get; set; }
        public string Durum { get; set; }
        public bool ElleGiris { get; set; }
        public string Not { get; set; }
        public string CalismaSuresi { get; set; }
        public int? CihazId { get; set; }
    }
}
