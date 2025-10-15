namespace PDKS.Business.DTOs
{
    // Dashboard DTO
    public class DashboardDTO
    {
        public int AktifPersonelSayisi { get; set; }
        public int BugunGelmeyenler { get; set; }
        public int GecGelenler { get; set; }
        public int ErkenCikanlar { get; set; }
        public int MesaiyeKalanlar { get; set; }
        public int IzinliPersonel { get; set; }
        public int BekleyenIzinTalepleri { get; set; }
        public int AktifCihazSayisi { get; set; }
        public List<GirisCikisListDTO> SonGirisCikislar { get; set; }
        public List<IzinListDTO> BekleyenIzinler { get; set; }
    }
}
