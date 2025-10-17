namespace PDKS.Business.DTOs
{
    public class IzinDetailDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string PersonelSicilNo { get; set; }
        public string IzinTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int GunSayisi { get; set; }
        public string OnayDurumu { get; set; }
        public string? OnaylayanKullaniciAdi { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string? Aciklama { get; set; }
        public string? RedNedeni { get; set; }
        public DateTime TalepTarihi { get; set; }
    }
}