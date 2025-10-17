namespace PDKS.Business.DTOs
{
    public class VardiyaDetailDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string BaslangicSaati { get; set; }
        public string BitisSaati { get; set; }
        public bool GeceVardiyasiMi { get; set; }
        public bool EsnekVardiyaMi { get; set; }
        public int ToleransSuresiDakika { get; set; }
        public string? Aciklama { get; set; }
        public bool Durum { get; set; }
    }
}