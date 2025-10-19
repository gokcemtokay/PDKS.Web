using System;

namespace PDKS.Business.DTOs
{
    public class KullaniciListDTO
    {
        public int Id { get; set; }
        public string PersonelAdi { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public bool Aktif { get; set; }

        // --- Eski View'ların Neden Olduğu Hataları Gidermek İçin Eklendi ---
        public string KullaniciAdi { get; set; } // PersonelAdi ile aynı değeri alacak
        public string PersonelSicilNo { get; set; }
        public string RolAdi { get; set; } // Rol ile aynı değeri alacak
        public DateTime? SonGirisTarihi { get; set; }
    }
}