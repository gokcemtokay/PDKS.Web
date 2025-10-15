namespace PDKS.Business.DTOs
{
    public class PersonelDetailDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Gorev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool Durum { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime? CikisTarihi { get; set; }
        public decimal Maas { get; set; }
        public decimal AvansLimiti { get; set; }
        public int? VardiyaId { get; set; }
        public string VardiyaAdi { get; set; }
        public int ToplamGirisCikis { get; set; }
        public int ToplamIzin { get; set; }
        public int BekleyenIzin { get; set; }
        public decimal ToplamAvans { get; set; }
    }
}
