namespace PDKS.Business.DTOs
{
    // İşe Girenler Rapor DTO
    public class IseGirenlerRaporDTO
    {
        public string AdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Gorev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public DateTime GirisTarihi { get; set; }
        public string VardiyaAdi { get; set; }
    }
}
