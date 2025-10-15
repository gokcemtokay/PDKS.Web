namespace PDKS.Business.DTOs
{
    // Fazla Mesai Rapor DTO
    public class FazlaMesaiRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public int FazlaMesaiSuresi { get; set; }
        public string FazlaMesaiSuresiText => $"{FazlaMesaiSuresi / 60}s {FazlaMesaiSuresi % 60}d";
        public decimal FazlaMesaiUcreti { get; set; }
    }
}
