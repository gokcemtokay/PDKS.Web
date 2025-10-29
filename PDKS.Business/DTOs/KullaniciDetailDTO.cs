namespace PDKS.Business.DTOs
{
    public class KullaniciDetailDTO
    {
        
        public int SirketId { get; set; }
public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public int? PersonelId { get; set; }
        public string? PersonelAdi { get; set; }
        public string? PersonelSicilNo { get; set; }
        public string? PersonelEmail { get; set; }
        public int RolId { get; set; }
        public string RolAdi { get; set; }
        public bool Aktif { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}