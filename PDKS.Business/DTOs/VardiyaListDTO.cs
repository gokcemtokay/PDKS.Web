namespace PDKS.Business.DTOs
{
    // Vardiya DTOs
    public class VardiyaListDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public string Ad { get; set; }
        public string BaslangicSaati { get; set; } = string.Empty;
        public string BitisSaati { get; set; } = string.Empty;
        public bool GeceVardiyasiMi { get; set; }
        public bool EsnekVardiyaMi { get; set; }
        public int ToleransSuresiDakika { get; set; }
        public string Aciklama { get; set; }
        public bool Durum { get; set; }
        public int PersonelSayisi { get; set; }
    }
}
