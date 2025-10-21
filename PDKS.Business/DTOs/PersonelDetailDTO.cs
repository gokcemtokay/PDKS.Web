namespace PDKS.Business.DTOs
{
    public class PersonelDetailDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public string SirketAdi { get; set; }
        public string AdSoyad { get; set; }
        public string Ad { get; set; }      // ✅ Ekleyin
        public string Soyad { get; set; }   // ✅ Ekleyin
        public string SicilNo { get; set; }
        public string TcKimlikNo { get; set; }
        public string Email { get; set; }
        public string? Telefon { get; set; }
        public string? Adres { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string? Cinsiyet { get; set; }
        public string? KanGrubu { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime? CikisTarihi { get; set; }
        public decimal Maas { get; set; }
        public string? Unvan { get; set; }
        public string? Gorev { get; set; }
        public decimal AvansLimiti { get; set; }
        public bool Durum { get; set; }

        // Departman
        public int? DepartmanId { get; set; }
        public string? DepartmanAdi { get; set; }
        public string Departman { get; set; }  // Geriye dönük uyumluluk için

        // Vardiya
        public int? VardiyaId { get; set; }
        public string? VardiyaAdi { get; set; }

        // İstatistikler
        public int ToplamGirisCikis { get; set; }
        public int ToplamIzin { get; set; }
        public int BekleyenIzin { get; set; }
        public int AktifAvansSayisi { get; set; }
        public decimal ToplamAvans { get; set; }
        public decimal ToplamPrim { get; set; }

        // Diğer
        public string? Notlar { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}