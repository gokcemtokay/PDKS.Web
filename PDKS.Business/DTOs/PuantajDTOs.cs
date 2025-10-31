using System;
using System.Collections.Generic;

namespace PDKS.Business.DTOs
{
    // Puantaj Listesi DTO
    public class PuantajListDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public string Donem => $"{Yil}/{Ay:00}";
        public int ToplamCalisilanGun { get; set; }
        public decimal ToplamCalismaSaati => ToplamCalismaSuresi / 60.0m;
        public int ToplamCalismaSuresi { get; set; }
        public decimal FazlaMesaiSaati => FazlaMesaiSuresi / 60.0m;
        public int FazlaMesaiSuresi { get; set; }
        public int DevamsizlikGunSayisi { get; set; }
        public int IzinGunSayisi { get; set; }
        public string Durum { get; set; }
        public bool Onaylandi { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

    // Puantaj Detay DTO
    public class PuantajDetailDTO
    {
        public int Id { get; set; }
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string VardiyaAdi { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }

        // Çalışma İstatistikleri
        public int ToplamCalismaSuresi { get; set; }
        public int NormalMesaiSuresi { get; set; }
        public int FazlaMesaiSuresi { get; set; }
        public int GeceMesaiSuresi { get; set; }
        public int HaftaTatiliCalismaSuresi { get; set; }
        public int ResmiTatilCalismaSuresi { get; set; }

        // Gün Sayıları
        public int ToplamCalisilanGun { get; set; }
        public int GecKalmaGunSayisi { get; set; }
        public int ErkenCikisGunSayisi { get; set; }
        public int DevamsizlikGunSayisi { get; set; }
        public int IzinGunSayisi { get; set; }
        public int HastaTatiliGunSayisi { get; set; }
        public int MazeretliIzinGunSayisi { get; set; }
        public int UcretsizIzinGunSayisi { get; set; }
        public int HaftaTatiliGunSayisi { get; set; }
        public int ResmiTatilGunSayisi { get; set; }

        // Süre Detayları
        public int ToplamGecKalmaSuresi { get; set; }
        public int ToplamErkenCikisSuresi { get; set; }
        public int ToplamEksikCalismaSuresi { get; set; }

        // Durum
        public string Durum { get; set; }
        public bool Onaylandi { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public string OnaylayanAdSoyad { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string Notlar { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public List<PuantajDetayItemDTO> Detaylar { get; set; }
    }

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

    // Puantaj Oluşturma DTO
    public class PuantajCreateDTO
    {
        public int SirketId { get; set; }
        public int PersonelId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public string Notlar { get; set; }
    }

    // Toplu Puantaj Oluşturma DTO
    public class PuantajTopluOlusturDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public List<int> PersonelIdler { get; set; }
        public bool TumPersoneller { get; set; }
        public int? DepartmanId { get; set; }
    }

    // Puantaj Onay DTO
    public class PuantajOnayDTO
    {
        public int PuantajId { get; set; }
        public bool Onayla { get; set; }
        public string Notlar { get; set; }
    }

    // Puantaj İstatistik DTO
    public class PuantajIstatistikDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public int ToplamPersonelSayisi { get; set; }
        public int PuantajHesaplananSayisi { get; set; }
        public int PuantajOnayl ananSayisi { get; set; }
        public int ToplamCalismaSuresi { get; set; }
        public int ToplamFazlaMesaiSuresi { get; set; }
        public int ToplamDevamsizlikGunSayisi { get; set; }
        public int ToplamIzinGunSayisi { get; set; }
    }

    // Puantaj Rapor Filtre DTO
    public class PuantajRaporFiltreDTO
    {
        public int SirketId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public int? DepartmanId { get; set; }
        public int? PersonelId { get; set; }
        public bool? SadeceOnaylanan { get; set; }
        public string RaporTipi { get; set; } // Ozet, Detayli, FazlaMesai, Devamsizlik, vb.
    }

    // Geç Kalanlar Rapor DTO
    public class GecKalanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public TimeSpan VardiyaBaslangic { get; set; }
        public DateTime? GirisSaati { get; set; }
        public int GecKalmaSuresi { get; set; }
        public string GecKalmaSuresiStr => $"{GecKalmaSuresi / 60}:{GecKalmaSuresi % 60:00}";
    }

    // Erken Çıkanlar Rapor DTO
    public class ErkenCikanlarRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public TimeSpan VardiyaBitis { get; set; }
        public DateTime? CikisSaati { get; set; }
        public int ErkenCikisSuresi { get; set; }
        public string ErkenCikisSuresiStr => $"{ErkenCikisSuresi / 60}:{ErkenCikisSuresi % 60:00}";
    }

    // Fazla Mesai Rapor DTO
    public class FazlaMesaiRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public int FazlaMesaiSuresi { get; set; }
        public string FazlaMesaiSuresiStr => $"{FazlaMesaiSuresi / 60}:{FazlaMesaiSuresi % 60:00}";
        public bool HaftaTatiliMi { get; set; }
        public bool ResmiTatilMi { get; set; }
    }

    // Devamsızlık Rapor DTO
    public class DevamsizlikRaporDTO
    {
        public DateTime Tarih { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string DepartmanAdi { get; set; }
        public string DevamsizlikNedeni { get; set; }
    }
}
