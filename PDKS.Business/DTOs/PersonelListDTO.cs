namespace PDKS.Business.DTOs
{
    // Personel DTOs
    public class PersonelListDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Gorev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool Durum { get; set; }
        public string DurumText => Durum ? "Aktif" : "Pasif";
        public DateTime GirisTarihi { get; set; }
        public DateTime? CikisTarihi { get; set; }
        public string VardiyaAdi { get; set; }
    }
}
