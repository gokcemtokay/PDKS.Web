using System;

namespace PDKS.Data.Entities
{
    public class DeviceToken
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public string Token { get; set; } // FCM Token
        public string? DeviceInfo { get; set; } // Model, OS version
        public string Platform { get; set; } // iOS, Android
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime SonKullanimTarihi { get; set; }
        public bool Aktif { get; set; }

        // Navigation
        public virtual Kullanici Kullanici { get; set; }
    }
}