namespace PDKS.Business.DTOs
{
    // İşten Ayrılanlar Rapor DTO
    public class IstenAyrilanlarRaporDTO
    {
        
        public int SirketId { get; set; }
public string AdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Gorev { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public int CalismaSuresi { get; set; }
        public string CalismaSuresiText => $"{CalismaSuresi} gün";
    }
}
