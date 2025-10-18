namespace PDKS.Business.DTOs
{
    // Kart Unutanlar Rapor DTO
    public class KartUnutanlarRaporDTO
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisSaati { get; set; }
        public DateTime? CikisSaati { get; set; }
        public DateTime GirisZamani { get; set; } // ✅ Eklendi
        public DateTime? CikisZamani { get; set; } // ✅ Eklendi
        public string Not { get; set; }
        public string OnaylayanAdi { get; set; }
        public bool ElleGiris { get; set; }
        public DateTime KayitZamani { get; set; }
    }
}
