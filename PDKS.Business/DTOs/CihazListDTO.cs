namespace PDKS.Business.DTOs
{
    // Cihaz DTOs
    public class CihazListDTO
    {
        public int Id { get; set; }
        public string CihazAdi { get; set; }
        public string IPAdres { get; set; }
        public string Lokasyon { get; set; }
        public bool Durum { get; set; }
        public string DurumText => Durum ? "Aktif" : "Pasif";
        public DateTime? SonBaglantiZamani { get; set; }
        public int BugunkuOkumaSayisi { get; set; }
    }
}
