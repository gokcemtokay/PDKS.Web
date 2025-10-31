namespace PDKS.Business.DTOs
{
    // List DTO - Puantaj listesi için
    public class PuantajListDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public int? DepartmanId { get; set; }
        public string Departman { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public string Donem { get; set; } // "Ocak 2025" formatında
        public int ToplamCalismaSaati { get; set; }
        public int ToplamCalisilanGun { get; set; }
        public int FazlaMesaiSaati { get; set; }
        public int DevamsizlikGunu { get; set; }
        public int IzinGunu { get; set; }
        public string Durum { get; set; }
        public DateTime? OnayTarihi { get; set; }
    }

    // Detail DTO - Detaylı puantaj görünümü için
    public class PuantajDetailDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdi { get; set; }
        public string SicilNo { get; set; }
        public string Departman { get; set; }
        public string Unvan { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        
        // Çalışma İstatistikleri
        public int ToplamCalismaSaati { get; set; }
        public decimal ToplamCalismaSaatFormatli => ToplamCalismaSaati / 60m; // Saat cinsinden
        public int NormalMesaiSaati { get; set; }
        public int FazlaMesaiSaati { get; set; }
        public int GeceMesaiSaati { get; set; }
        public int HaftaSonuMesaiSaati { get; set; }
        
        // Devamsızlık ve İzin
        public int ToplamCalisilanGun { get; set; }
        public int DevamsizlikGunu { get; set; }
        public int IzinGunu { get; set; }
        public int RaporluGun { get; set; }
        public int HaftaTatiliGunu { get; set; }
        public int ResmiTatilGunu { get; set; }
        
        // Geç Kalma ve Erken Çıkış
        public int GecKalmaGunu { get; set; }
        public int GecKalmaSuresi { get; set; }
        public int ErkenCikisGunu { get; set; }
        public int ErkenCikisSuresi { get; set; }
        public int EksikCalismaSaati { get; set; }
        
        public string Durum { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string OnaylayanKisi { get; set; }
        public string Notlar { get; set; }
        
        // Günlük detaylar
        public List<PuantajDetayDTO> GunlukDetaylar { get; set; }
    }

    // Puantaj hesaplama için
    public class PuantajHesaplaDTO
    {
        public int PersonelId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public bool YenidenHesapla { get; set; } = false;
    }

    // Toplu puantaj hesaplama
    public class TopluPuantajHesaplaDTO
    {
        public List<int> PersonelIdler { get; set; }
        public int? DepartmanId { get; set; }
        public int Yil { get; set; }
        public int Ay { get; set; }
        public bool TumPersonel { get; set; } = false;
    }

    // Onaylama için
    public class PuantajOnayDTO
    {
        public int PuantajId { get; set; }
        public int OnaylayanKullaniciId { get; set; }
        public string? Notlar { get; set; }
    }

    // Günlük detay DTO
    public class PuantajDetayDTO
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Gun { get; set; } // Pazartesi, Salı vb.
        public string VardiyaAdi { get; set; }
        public TimeSpan? PlanlananGiris { get; set; }
        public TimeSpan? PlanlananCikis { get; set; }
        public DateTime? GerceklesenGiris { get; set; }
        public DateTime? GerceklesenCikis { get; set; }
        public int? GerceklesenSure { get; set; }
        public int? NormalMesai { get; set; }
        public int? FazlaMesai { get; set; }
        public int? GecKalmaSuresi { get; set; }
        public int? ErkenCikisSuresi { get; set; }
        public string GunDurumu { get; set; }
        public string? IzinTuru { get; set; }
        public bool HaftaSonuMu { get; set; }
        public bool ResmiTatilMi { get; set; }
        public bool GecKaldiMi { get; set; }
        public bool ErkenCiktiMi { get; set; }
        public bool DevamsizMi { get; set; }
        public string? Notlar { get; set; }
    }

    // Rapor DTO'ları
    public class PuantajRaporParametreDTO
    {
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int? PersonelId { get; set; }
        public int? DepartmanId { get; set; }
        public List<int>? PersonelIdler { get; set; }
        public string? RaporTuru { get; set; } // Ozet, Detayli, Karsilastirmali
    }

    public class PuantajOzetRaporDTO
    {
        public string Donem { get; set; }
        public int ToplamPersonel { get; set; }
        public int ToplamCalismaSaati { get; set; }
        public int ToplamFazlaMesai { get; set; }
        public int ToplamDevamsizlik { get; set; }
        public List<PuantajListDTO> PersonelPuantajlari { get; set; }
    }

    public class DepartmanPuantajOzetDTO
    {
        public int DepartmanId { get; set; }
        public string DepartmanAdi { get; set; }
        public int PersonelSayisi { get; set; }
        public int ToplamCalismaSaati { get; set; }
        public int ToplamFazlaMesai { get; set; }
        public int ToplamDevamsizlik { get; set; }
        public decimal OrtalamaCalismaOrani { get; set; }
    }
}
