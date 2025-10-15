namespace PDKS.Business.DTOs
{
    // Vardiya DTOs
    public class VardiyaListDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }
        public bool GeceVardiyasiMi { get; set; }
        public bool EsnekVardiyaMi { get; set; }
        public int ToleransSuresiDakika { get; set; }
        public string Aciklama { get; set; }
        public bool Durum { get; set; }
        public int PersonelSayisi { get; set; }
    }
}
