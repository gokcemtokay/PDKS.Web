namespace PDKS.Business.DTOs
{
    public class DepartmanListDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? Kod { get; set; }
        public string? Aciklama { get; set; }
        public int? UstDepartmanId { get; set; }
        public string? UstDepartmanAdi { get; set; }
        public bool Durum { get; set; }
        public int PersonelSayisi { get; set; }
    }
}