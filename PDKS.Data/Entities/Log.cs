using System;

namespace PDKS.Data.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int? KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
        public string Islem { get; set; } // "Create", "Update", "Login" vb. (IslemTuru -> Islem)
        public string Aciklama { get; set; } // What was done (Detay -> Aciklama)
        public string IpAdresi { get; set; } // (IpAdres -> IpAdresi)
        public string LogLevel { get; set; } // "Info", "Warning", "Error"
    }
}