namespace PDKS.Business.DTOs
{
    public class MesaiListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string PersonelSicilNo { get; set; }
        public DateTime Tarih { get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }
        public decimal ToplamSaat { get; set; }
        public decimal FazlaMesaiSaati { get; set; }
        public string MesaiTipi { get; set; }
        public string OnayDurumu { get; set; }
        public string? OnaylayanKullaniciAdi { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string? Aciklama { get; set; }
    }
}