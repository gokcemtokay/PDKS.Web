using System;

namespace PDKS.Business.DTOs
{
    public class KullaniciListDTO
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;           // ✅ VAR
        public string Soyad { get; set; } = string.Empty;        // ✅ VAR
        public string AdSoyad => $"{Ad} {Soyad}";
        public string Email { get; set; } = string.Empty;        // ✅ VAR
        public int RolId { get; set; }
        public string RolAdi { get; set; } = string.Empty;
        public bool Aktif { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
        public DateTime KayitTarihi { get; set; }

        // ✅ YETKİLİ ŞİRKETLER
        public List<KullaniciSirketDTO> YetkiliSirketler { get; set; } = new List<KullaniciSirketDTO>();  // ✅ VAR
    }

}