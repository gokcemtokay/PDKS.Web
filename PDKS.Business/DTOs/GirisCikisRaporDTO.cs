// ============================================
// GirisCikisRaporDTO.cs
// Dosya: PDKS.Business/DTOs/GirisCikisRaporDTO.cs
// ============================================
using System;

namespace PDKS.Business.DTOs
{
    public class GirisCikisRaporDTO
    {
        // Temel Bilgiler
        public int Id { get; set; }
        public DateTime Tarih { get; set; }

        // Personel Bilgileri
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }

        // Giriş-Çıkış Saatleri
        public DateTime? GirisSaati { get; set; }
        public DateTime? CikisSaati { get; set; }

        // Hesaplanan Değerler
        public int ToplamCalismaDakika { get; set; }
        public int GecKalma { get; set; }  // Dakika cinsinden
        public int ErkenCikis { get; set; } // Dakika cinsinden
        public int FazlaMesaiDakika { get; set; }

        // Vardiya Bilgisi
        public string VardiyaAdi { get; set; }
        public TimeSpan? VardiyaBaslangic { get; set; }
        public TimeSpan? VardiyaBitis { get; set; }

        // Diğer
        public string Durum { get; set; } // "Normal", "Geç", "Erken Çıkış", "Devamsız"
        public string Aciklama { get; set; }
        public bool TatilGunu { get; set; }
        public bool HaftaSonu { get; set; }

        // Computed Properties (Kullanım kolaylığı için)
        public string ToplamCalismaSuresi => TimeSpan.FromMinutes(ToplamCalismaDakika).ToString(@"hh\:mm");
        public string GecKalmaSuresi => GecKalma > 0 ? $"{GecKalma} dk" : "-";
        public string ErkenCikisSuresi => ErkenCikis > 0 ? $"{ErkenCikis} dk" : "-";
    }
}