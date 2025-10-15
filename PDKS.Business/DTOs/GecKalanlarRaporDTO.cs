namespace PDKS.Business.DTOs
{
    // Geç Kalanlar Rapor DTO
    public class GecKalanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string BeklenenGiris { get; set; }
        public string GercekGiris { get; set; }
        public int GecKalmaSuresi { get; set; }
        public string GecKalmaSuresiText => $"{GecKalmaSuresi / 60}s {GecKalmaSuresi % 60}d";
    }
}
