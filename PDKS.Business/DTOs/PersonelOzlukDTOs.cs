namespace PDKS.Business.DTOs
{
    // ==================== AİLE BİLGİLERİ ====================
    public class PersonelAileDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string YakinlikDerecesi { get; set; }
        public string AdSoyad { get; set; }
        public string? TcKimlikNo { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string? Meslek { get; set; }
        public bool CalisiyorMu { get; set; }
        public string? Telefon { get; set; }
        public bool OgrenciMi { get; set; }
        public bool SGKBagimlisi { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelAileCreateDTO
    {
        public int PersonelId { get; set; }
        public string YakinlikDerecesi { get; set; }
        public string AdSoyad { get; set; }
        public string? TcKimlikNo { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string? Meslek { get; set; }
        public bool CalisiyorMu { get; set; }
        public string? Telefon { get; set; }
        public bool OgrenciMi { get; set; }
        public bool SGKBagimlisi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== ACİL DURUM ====================
    public class PersonelAcilDurumDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string IletisimTipi { get; set; }
        public string AdSoyad { get; set; }
        public string YakinlikDerecesi { get; set; }
        public string Telefon1 { get; set; }
        public string? Telefon2 { get; set; }
        public string? Adres { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelAcilDurumCreateDTO
    {
        public int PersonelId { get; set; }
        public string IletisimTipi { get; set; }
        public string AdSoyad { get; set; }
        public string YakinlikDerecesi { get; set; }
        public string Telefon1 { get; set; }
        public string? Telefon2 { get; set; }
        public string? Adres { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== SAĞLIK ====================
    public class PersonelSaglikDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string? KanGrubu { get; set; }
        public int? Boy { get; set; }
        public decimal? Kilo { get; set; }
        public string? KronikHastaliklar { get; set; }
        public string? Alerjiler { get; set; }
        public string? SurekliKullanilanIlaclar { get; set; }
        public bool EngelDurumuVarMi { get; set; }
        public int? EngelYuzdesi { get; set; }
        public string? EngelAciklama { get; set; }
        public string? SaglikRaporlari { get; set; }
        public DateTime? SonPeriyodikMuayeneTarihi { get; set; }
        public DateTime? SonradakiPeriyodikMuayeneTarihi { get; set; }
        public DateTime? IsGuvenligiEgitimiTarihi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== EĞİTİM GEÇMİŞİ ====================
    public class PersonelEgitimDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string EgitimSeviyesi { get; set; }
        public string OkulAdi { get; set; }
        public string? Bolum { get; set; }
        public int BaslangicYili { get; set; }
        public int? BitisYili { get; set; }
        public string MezuniyetDurumu { get; set; }
        public decimal? MezuniyetNotu { get; set; }
        public string? DiplomaDosyasi { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelEgitimCreateDTO
    {
        public int PersonelId { get; set; }
        public string EgitimSeviyesi { get; set; }
        public string OkulAdi { get; set; }
        public string? Bolum { get; set; }
        public int BaslangicYili { get; set; }
        public int? BitisYili { get; set; }
        public string MezuniyetDurumu { get; set; }
        public decimal? MezuniyetNotu { get; set; }
        public string? DiplomaDosyasi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== İŞ DENEYİMİ ====================
    public class PersonelIsDeneyimiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string SirketAdi { get; set; }
        public string Pozisyon { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public bool HalenCalisiyor { get; set; }
        public string? IsTanimi { get; set; }
        public string? AyrilmaNedeni { get; set; }
        public string? ReferansKisiAdi { get; set; }
        public string? ReferansKisiTelefon { get; set; }
        public string? ReferansKisiEmail { get; set; }
        public bool SGKTescilliMi { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelIsDeneyimiCreateDTO
    {
        public int PersonelId { get; set; }
        public string SirketAdi { get; set; }
        public string Pozisyon { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public bool HalenCalisiyor { get; set; }
        public string? IsTanimi { get; set; }
        public string? AyrilmaNedeni { get; set; }
        public string? ReferansKisiAdi { get; set; }
        public string? ReferansKisiTelefon { get; set; }
        public string? ReferansKisiEmail { get; set; }
        public bool SGKTescilliMi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== DİL BECERİLERİ ====================
    public class PersonelDilDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string DilAdi { get; set; }
        public string Seviye { get; set; }
        public int OkumaSeviyesi { get; set; }
        public int YazmaSeviyesi { get; set; }
        public int KonusmaSeviyesi { get; set; }
        public string? SertifikaTuru { get; set; }
        public int? SertifikaPuani { get; set; }
        public DateTime? SertifikaTarihi { get; set; }
        public string? SertifikaDosyasi { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelDilCreateDTO
    {
        public int PersonelId { get; set; }
        public string DilAdi { get; set; }
        public string Seviye { get; set; }
        public int OkumaSeviyesi { get; set; } = 1;
        public int YazmaSeviyesi { get; set; } = 1;
        public int KonusmaSeviyesi { get; set; } = 1;
        public string? SertifikaTuru { get; set; }
        public int? SertifikaPuani { get; set; }
        public DateTime? SertifikaTarihi { get; set; }
        public string? SertifikaDosyasi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== SERTİFİKALAR ====================
    public class PersonelSertifikaDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string SertifikaAdi { get; set; }
        public string VerenKurum { get; set; }
        public DateTime AlimTarihi { get; set; }
        public DateTime? GecerlilikTarihi { get; set; }
        public bool SureliMi { get; set; }
        public string? SertifikaNumarasi { get; set; }
        public string? SertifikaDosyasi { get; set; }
        public string Durum { get; set; }
        public bool HatirlatmaGonderildiMi { get; set; }
        public DateTime? HatirlatmaTarihi { get; set; }
        public string? Notlar { get; set; }
        public int? KalanGunSayisi { get; set; } // Hesaplanacak
    }

    public class PersonelSertifikaCreateDTO
    {
        public int PersonelId { get; set; }
        public string SertifikaAdi { get; set; }
        public string VerenKurum { get; set; }
        public DateTime AlimTarihi { get; set; }
        public DateTime? GecerlilikTarihi { get; set; }
        public bool SureliMi { get; set; }
        public string? SertifikaNumarasi { get; set; }
        public string? SertifikaDosyasi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== PERFORMANS ====================
    public class PersonelPerformansDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public DateTime DegerlendirmeTarihi { get; set; }
        public string Donem { get; set; }
        public int DegerlendiriciKullaniciId { get; set; }
        public string DegerlendiriciAdSoyad { get; set; }
        public decimal PerformansNotu { get; set; }
        public string? NotSkalasi { get; set; }
        public string? Hedefler { get; set; }
        public decimal? HedefBasariOrani { get; set; }
        public string? GucluYonler { get; set; }
        public string? GelisimAlanlari { get; set; }
        public string? Yorumlar { get; set; }
        public string? AksiyonPlanlari { get; set; }
        public string Durum { get; set; }
        public int? OnaylayanKullaniciId { get; set; }
        public string? OnaylayanAdSoyad { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelPerformansCreateDTO
    {
        public int PersonelId { get; set; }
        public DateTime DegerlendirmeTarihi { get; set; }
        public string Donem { get; set; }
        public int DegerlendiriciKullaniciId { get; set; }
        public decimal PerformansNotu { get; set; }
        public string? NotSkalasi { get; set; }
        public string? Hedefler { get; set; }
        public decimal? HedefBasariOrani { get; set; }
        public string? GucluYonler { get; set; }
        public string? GelisimAlanlari { get; set; }
        public string? Yorumlar { get; set; }
        public string? AksiyonPlanlari { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== DİSİPLİN ====================
    public class PersonelDisiplinDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string DisiplinTuru { get; set; }
        public DateTime OlayTarihi { get; set; }
        public string Aciklama { get; set; }
        public string? UygulananCeza { get; set; }
        public int KararVerenYetkiliId { get; set; }
        public string KararVerenYetkiliAdi { get; set; }
        public string? IlgiliDokumanlar { get; set; }
        public string Durum { get; set; }
        public DateTime? IptalTarihi { get; set; }
        public string? IptalNedeni { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelDisiplinCreateDTO
    {
        public int PersonelId { get; set; }
        public string DisiplinTuru { get; set; }
        public DateTime OlayTarihi { get; set; }
        public string Aciklama { get; set; }
        public string? UygulananCeza { get; set; }
        public int KararVerenYetkiliId { get; set; }
        public string? IlgiliDokumanlar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== TERFİ ====================
    public class PersonelTerfiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public DateTime TerfiTarihi { get; set; }
        public string? EskiPozisyon { get; set; }
        public string YeniPozisyon { get; set; }
        public string? EskiUnvan { get; set; }
        public string YeniUnvan { get; set; }
        public int? EskiDepartmanId { get; set; }
        public string? EskiDepartmanAdi { get; set; }
        public int? YeniDepartmanId { get; set; }
        public string? YeniDepartmanAdi { get; set; }
        public string? TerfiNedeni { get; set; }
        public int OnaylayanKullaniciId { get; set; }
        public string OnaylayanAdSoyad { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelTerfiCreateDTO
    {
        public int PersonelId { get; set; }
        public DateTime TerfiTarihi { get; set; }
        public string? EskiPozisyon { get; set; }
        public string YeniPozisyon { get; set; }
        public string? EskiUnvan { get; set; }
        public string YeniUnvan { get; set; }
        public int? EskiDepartmanId { get; set; }
        public int? YeniDepartmanId { get; set; }
        public string? TerfiNedeni { get; set; }
        public int OnaylayanKullaniciId { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== ÜCRET DEĞİŞİKLİK ====================
    public class PersonelUcretDegisiklikDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public DateTime DegisiklikTarihi { get; set; }
        public decimal EskiMaas { get; set; }
        public decimal YeniMaas { get; set; }
        public decimal DegisimYuzdesi { get; set; }
        public decimal FarkTutari { get; set; }
        public string DegisimNedeni { get; set; }
        public string? Aciklama { get; set; }
        public int OnaylayanKullaniciId { get; set; }
        public string OnaylayanAdSoyad { get; set; }
        public DateTime? OnayTarihi { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelUcretDegisiklikCreateDTO
    {
        public int PersonelId { get; set; }
        public DateTime DegisiklikTarihi { get; set; }
        public decimal EskiMaas { get; set; }
        public decimal YeniMaas { get; set; }
        public string DegisimNedeni { get; set; }
        public string? Aciklama { get; set; }
        public int OnaylayanKullaniciId { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== REFERANS ====================
    public class PersonelReferansDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string? SirketKurum { get; set; }
        public string? Pozisyon { get; set; }
        public string Iliski { get; set; }
        public string Telefon { get; set; }
        public string? Email { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelReferansCreateDTO
    {
        public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public string? SirketKurum { get; set; }
        public string? Pozisyon { get; set; }
        public string Iliski { get; set; }
        public string Telefon { get; set; }
        public string? Email { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== ZİMMET ====================
    public class PersonelZimmetDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string EsyaTipi { get; set; }
        public string EsyaAdi { get; set; }
        public string? MarkaModel { get; set; }
        public string? SeriNumarasi { get; set; }
        public DateTime ZimmetTarihi { get; set; }
        public DateTime? IadeTarihi { get; set; }
        public string ZimmetDurumu { get; set; }
        public decimal? Degeri { get; set; }
        public string? ZimmetSozlesmesi { get; set; }
        public string? ZimmetFotografi { get; set; }
        public string? Aciklama { get; set; }
        public int ZimmetVerenKullaniciId { get; set; }
        public string ZimmetVerenAdSoyad { get; set; }
        public int? IadeTeslimAlanKullaniciId { get; set; }
        public string? IadeTeslimAlanAdSoyad { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelZimmetCreateDTO
    {
        public int PersonelId { get; set; }
        public string EsyaTipi { get; set; }
        public string EsyaAdi { get; set; }
        public string? MarkaModel { get; set; }
        public string? SeriNumarasi { get; set; }
        public DateTime ZimmetTarihi { get; set; }
        public decimal? Degeri { get; set; }
        public string? ZimmetSozlesmesi { get; set; }
        public string? ZimmetFotografi { get; set; }
        public string? Aciklama { get; set; }
        public int ZimmetVerenKullaniciId { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== YETKİNLİK ====================
    public class PersonelYetkinlikDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string YetkinlikTipi { get; set; }
        public string YetkinlikAdi { get; set; }
        public string Seviye { get; set; }
        public int? SeviyePuani { get; set; }
        public DateTime? SonKullanimTarihi { get; set; }
        public bool SelfAssessment { get; set; }
        public int? DegerlendiriciKullaniciId { get; set; }
        public string? DegerlendiriciAdSoyad { get; set; }
        public DateTime? DegerlendirmeTarihi { get; set; }
        public string? BelgelendirenDokumanlar { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelYetkinlikCreateDTO
    {
        public int PersonelId { get; set; }
        public string YetkinlikTipi { get; set; }
        public string YetkinlikAdi { get; set; }
        public string Seviye { get; set; }
        public int? SeviyePuani { get; set; }
        public DateTime? SonKullanimTarihi { get; set; }
        public bool SelfAssessment { get; set; } = true;
        public int? DegerlendiriciKullaniciId { get; set; }
        public string? BelgelendirenDokumanlar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== EĞİTİM KAYIT ====================
    public class PersonelEgitimKayitDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string EgitimAdi { get; set; }
        public string? EgitmenKurum { get; set; }
        public DateTime EgitimTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public int? EgitimSuresiSaat { get; set; }
        public string TamamlanmaDurumu { get; set; }
        public decimal? EgitimMaliyeti { get; set; }
        public string? EgitimSertifikasi { get; set; }
        public bool SertifikaAldiMi { get; set; }
        public string? EgitimTuru { get; set; }
        public string? EgitimKategorisi { get; set; }
        public string? EgitimIcerigi { get; set; }
        public int? DegerlendirmePuani { get; set; }
        public string? PersonelGeribildirimi { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    public class PersonelEgitimKayitCreateDTO
    {
        public int PersonelId { get; set; }
        public string EgitimAdi { get; set; }
        public string? EgitmenKurum { get; set; }
        public DateTime EgitimTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public int? EgitimSuresiSaat { get; set; }
        public string TamamlanmaDurumu { get; set; } = "Tamamlandı";
        public decimal? EgitimMaliyeti { get; set; }
        public string? EgitimSertifikasi { get; set; }
        public bool SertifikaAldiMi { get; set; }
        public string? EgitimTuru { get; set; }
        public string? EgitimKategorisi { get; set; }
        public string? EgitimIcerigi { get; set; }
        public int? DegerlendirmePuani { get; set; }
        public string? PersonelGeribildirimi { get; set; }
        public string? EkDosyalar { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== MALİ BİLGİ ====================
    public class PersonelMaliBilgiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string? BankaAdi { get; set; }
        public string? IBAN { get; set; }
        public string? HesapTuru { get; set; }
        public string? VergiNo { get; set; }
        public string? VergiDairesi { get; set; }
        public string? SGKNo { get; set; }
        public DateTime? SGKBaslangicTarihi { get; set; }
        public decimal? AsgariUcretMuafiyeti { get; set; }
        public decimal? GelirVergisiOrani { get; set; }
        public bool EmekliSandigi { get; set; }
        public string? OdemeYontemi { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== EK BİLGİ ====================
    public class PersonelEkBilgiDTO
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string? MedeniDurum { get; set; }
        public string? AskerlikDurumu { get; set; }
        public DateTime? AskerlikBaslangicTarihi { get; set; }
        public DateTime? AskerlikBitisTarihi { get; set; }
        public string? AskerlikYeri { get; set; }
        public string? AskerlikRutbesi { get; set; }
        public string? EhliyetSiniflari { get; set; }
        public DateTime? EhliyetAlisTarihi { get; set; }
        public DateTime? EhliyetGecerlilikTarihi { get; set; }
        public string? Uyruk { get; set; }
        public string? IkametIli { get; set; }
        public string? IkametIlce { get; set; }
        public string? IkametAdresi { get; set; }
        public string? DogumYeri { get; set; }
        public string? AnneAdi { get; set; }
        public string? BabaAdi { get; set; }
        public int? CocukSayisi { get; set; }
        public string? HobiIlgiAlanlari { get; set; }
        public string? SosyalGuvence { get; set; }
        public bool SigortaliMi { get; set; }
        public string? SigortaSirketi { get; set; }
        public string? SigortaPoliceNo { get; set; }
        public string? Notlar { get; set; }
    }

    // ==================== KOMBİNE DTO ====================
    public class PersonelOzlukDetayDTO
    {
        public int PersonelId { get; set; }
        public string AdSoyad { get; set; }
        public List<PersonelAileDTO> AileBilgileri { get; set; } = new();
        public List<PersonelAcilDurumDTO> AcilDurumBilgileri { get; set; } = new();
        public PersonelSaglikDTO? SaglikBilgisi { get; set; }
        public List<PersonelEgitimDTO> EgitimGecmisi { get; set; } = new();
        public List<PersonelIsDeneyimiDTO> IsDeneyimleri { get; set; } = new();
        public List<PersonelDilDTO> DilBecerileri { get; set; } = new();
        public List<PersonelSertifikaDTO> Sertifikalar { get; set; } = new();
        public List<PersonelPerformansDTO> PerformansKayitlari { get; set; } = new();
        public List<PersonelDisiplinDTO> DisiplinKayitlari { get; set; } = new();
        public List<PersonelTerfiDTO> TerfiGecmisi { get; set; } = new();
        public List<PersonelUcretDegisiklikDTO> UcretDegisiklikleri { get; set; } = new();
        public List<PersonelReferansDTO> Referanslar { get; set; } = new();
        public List<PersonelZimmetDTO> ZimmetliEsyalar { get; set; } = new();
        public List<PersonelYetkinlikDTO> Yetkinlikler { get; set; } = new();
        public List<PersonelEgitimKayitDTO> EgitimKayitlari { get; set; } = new();
        public PersonelMaliBilgiDTO? MaliBilgi { get; set; }
        public PersonelEkBilgiDTO? EkBilgi { get; set; }
    }
}
