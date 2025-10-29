namespace PDKS.Business.DTOs
{
    // İzinli Personel Rapor DTO
    public class IzinliPersonelRaporDTO
    {
        
        public int SirketId { get; set; }
public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string IzinTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int GunSayisi { get; set; }
        public string Aciklama { get; set; }
    }
}
