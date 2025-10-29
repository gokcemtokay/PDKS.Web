namespace PDKS.Business.DTOs
{
    public class CihazCreateDTO
    {
        
        public int SirketId { get; set; }
public string CihazAdi { get; set; }
        public string IPAdres { get; set; }
        public string Lokasyon { get; set; }
        public bool Durum { get; set; }
    }
}