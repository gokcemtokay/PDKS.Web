namespace PDKS.Business.DTOs
{
    // Avans DTOs
    public class AvansListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public decimal Tutar { get; set; }
        public DateTime TalepTarihi { get; set; }
        public DateTime? OdemeTarihi { get; set; }
        public string Durum { get; set; }
        public string Aciklama { get; set; }
        public string OnaylayanAdi { get; set; }
    }
}
