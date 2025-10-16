namespace PDKS.Business.DTOs
{
    // Prim DTOs
    public class PrimListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string Donem { get; set; }
        public decimal Tutar { get; set; }
        public string PrimTipi { get; set; }
        public string Aciklama { get; set; }
    }
}
