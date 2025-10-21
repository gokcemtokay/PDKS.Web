namespace PDKS.Business.DTOs
{
    public class PersonelListDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public string SirketAdi { get; set; }
        public string AdSoyad { get; set; }
        public string Ad { get; set; }      // ✅ Ekleyin
        public string Soyad { get; set; }   // ✅ Ekleyin
        public string SicilNo { get; set; }
        public string TcKimlikNo { get; set; } // ✅ Ekleyin (frontend'de kullanılıyor)
        public string Departman { get; set; }
        public string DepartmanAdi { get; set; } // ✅ Ekleyin (alias)
        public string Unvan { get; set; }
        public string Gorev { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public bool Durum { get; set; }
        public bool Aktif { get; set; } // ✅ Ekleyin (alias)
        public string DurumText => Durum ? "Aktif" : "Pasif";
        public DateTime GirisTarihi { get; set; }
        public DateTime? IseBaslamaTarihi { get; set; } // ✅ Ekleyin
        public DateTime? CikisTarihi { get; set; }
        public string VardiyaAdi { get; set; }
    }
}