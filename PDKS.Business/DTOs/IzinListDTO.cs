namespace PDKS.Business.DTOs
{
    // Izin DTOs
    public class IzinListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string IzinTipi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int GunSayisi { get; set; }
        public string Aciklama { get; set; }
        public string OnayDurumu { get; set; }
        public string OnaylayanAdi { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}
