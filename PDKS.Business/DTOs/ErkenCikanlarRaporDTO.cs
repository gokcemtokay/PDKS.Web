namespace PDKS.Business.DTOs
{
    // Erken Çıkanlar Rapor DTO
    public class ErkenCikanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string BeklenenCikis { get; set; }
        public string GercekCikis { get; set; }
        public int ErkenCikisSuresi { get; set; }
        public string ErkenCikisSuresiText => $"{ErkenCikisSuresi / 60}s {ErkenCikisSuresi % 60}d";
    }
}
