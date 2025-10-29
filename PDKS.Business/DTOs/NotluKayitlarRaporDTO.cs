namespace PDKS.Business.DTOs
{
    // Notlu Kayıtlar Rapor DTO
    public class NotluKayitlarRaporDTO
    {
        
        public int SirketId { get; set; }
public DateTime Tarih { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public DateTime? GirisZamani { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Not { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}
