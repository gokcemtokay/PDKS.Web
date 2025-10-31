using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.DTOs
{
    // Puantaj Günlük Detay DTO
    public class PuantajDetayItemDTO
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Gun => Tarih.ToString("dd.MM.yyyy");
        public string HaftaGunu => Tarih.ToString("dddd", new System.Globalization.CultureInfo("tr-TR"));
        public int? GunTipi { get; set; }
        public string GunTipiAdi => GunTipi switch
        {
            1 => "Hafta İçi",
            2 => "Hafta Tatili",
            3 => "Resmi Tatil",
            _ => "-"
        };

        public DateTime? IlkGiris { get; set; }
        public string IlkGirisStr => IlkGiris?.ToString("HH:mm") ?? "-";
        public DateTime? SonCikis { get; set; }
        public string SonCikisStr => SonCikis?.ToString("HH:mm") ?? "-";
        public int? ToplamCalismaSuresi { get; set; }
        public string ToplamCalismaSaati => ToplamCalismaSuresi.HasValue ? $"{ToplamCalismaSuresi.Value / 60}:{ToplamCalismaSuresi.Value % 60:00}" : "-";

        public TimeSpan? VardiyaBaslangic { get; set; }
        public TimeSpan? VardiyaBitis { get; set; }
        public int? PlanlananCalismaSuresi { get; set; }

        public string CalismaDurumu { get; set; }
        public int? GecKalmaSuresi { get; set; }
        public int? ErkenCikisSuresi { get; set; }
        public int? FazlaMesaiSuresi { get; set; }
        public int? EksikCalismaSuresi { get; set; }

        public bool IzinliMi { get; set; }
        public string IzinTuru { get; set; }
        public int? ToplamMolaSuresi { get; set; }
        public bool ElleGirildiMi { get; set; }
        public string Notlar { get; set; }
    }
}
