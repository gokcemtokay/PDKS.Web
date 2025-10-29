namespace PDKS.Business.DTOs
{
    public class DevamGunDTO
    {
        
        public int SirketId { get; set; }
public DateTime Tarih { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Durum { get; set; }
        public int CalismaSuresi { get; set; }
        public string CalismaSuresiText => CalismaSuresi > 0 ? $"{CalismaSuresi / 60}s {CalismaSuresi % 60}d" : "-";
    }
}
