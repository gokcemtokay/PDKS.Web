namespace PDKS.Business.DTOs
{
    public class KullaniciDetailDTO : KullaniciListDTO
    {
        public int? PersonelId { get; set; }
        public string? PersonelAdSoyad { get; set; }  // ✅ VAR
    }
}