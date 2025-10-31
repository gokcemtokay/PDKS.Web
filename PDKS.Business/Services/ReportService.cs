using PDKS.Business.DTOs;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GirisCikisRaporDTO>> KisiBazindaGirisCikisRaporu(int personelId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis);

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);

            return kayitlar.Select(k => new GirisCikisRaporDTO
            {
                Tarih = k.GirisZamani?.Date ?? DateTime.UtcNow,
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                GirisSaati = k.GirisZamani,
                CikisSaati = k.CikisZamani,
                ToplamCalismaDakika = CalculateWorkMinutes(k.GirisZamani, k.CikisZamani),
                Durum = k.Durum,
                GecKalma = k.GecKalmaSuresi ?? 0,
                ErkenCikis = k.ErkenCikisSuresi ?? 0,
                FazlaMesaiDakika = k.FazlaMesaiSuresi ?? 0
            }).OrderBy(r => r.Tarih).ToList();
        }

        public async Task<List<GirisCikisRaporDTO>> GenelBazdaGirisCikisRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis);

            var result = new List<GirisCikisRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new GirisCikisRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                    GirisSaati = kayit.GirisZamani,
                    CikisSaati = kayit.CikisZamani,
                    ToplamCalismaDakika = CalculateWorkMinutes(kayit.GirisZamani, kayit.CikisZamani),
                    Durum = kayit.Durum,
                    GecKalma = kayit.GecKalmaSuresi ?? 0,
                    ErkenCikis = kayit.ErkenCikisSuresi ?? 0,
                    FazlaMesaiDakika = kayit.FazlaMesaiSuresi ?? 0
                });
            }

            return result.OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdi).ToList();
        }

        public async Task<List<GecKalanlarRaporDTO>> KisiBazindaGecKalanlarRaporu(int personelId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.GecKalmaSuresi > 0);

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);

            return kayitlar.Select(k => new GecKalanlarRaporDTO
            {
                Tarih = k.GirisZamani?.Date ?? DateTime.UtcNow,
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                BeklenenGiris = personel.Vardiya?.BaslangicSaati.ToString(@"hh\:mm") ?? "",
                GercekGiris = k.GirisZamani?.ToString("HH:mm") ?? "",
                GecKalmaSuresi = k.GecKalmaSuresi ?? 0
            }).OrderByDescending(r => r.GecKalmaSuresi).ToList();
        }

        public async Task<List<GecKalanlarRaporDTO>> GenelBazdaGecKalanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.GecKalmaSuresi > 0);

            var result = new List<GecKalanlarRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new GecKalanlarRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                    BeklenenGiris = personel.Vardiya?.BaslangicSaati.ToString(@"hh\:mm") ?? "",
                    GercekGiris = kayit.GirisZamani?.ToString("HH:mm") ?? "",
                    GecKalmaSuresi = kayit.GecKalmaSuresi ?? 0
                });
            }

            return result.OrderByDescending(r => r.GecKalmaSuresi).ToList();
        }

        public async Task<List<ErkenCikanlarRaporDTO>> KisiBazindaErkenCikanlarRaporu(int personelId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.ErkenCikisSuresi > 0);

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);

            return kayitlar.Select(k => new ErkenCikanlarRaporDTO
            {
                Tarih = k.GirisZamani?.Date ?? DateTime.UtcNow,
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  
                BeklenenCikis = personel.Vardiya?.BitisSaati.ToString(@"hh\:mm") ?? "",
                GercekCikis = k.CikisZamani?.ToString("HH:mm") ?? "",
                ErkenCikisSuresi = k.ErkenCikisSuresi ?? 0
            }).OrderByDescending(r => r.ErkenCikisSuresi).ToList();
        }

        public async Task<List<ErkenCikanlarRaporDTO>> GenelBazdaErkenCikanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.ErkenCikisSuresi > 0);

            var result = new List<ErkenCikanlarRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new ErkenCikanlarRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                    BeklenenCikis = personel.Vardiya?.BitisSaati.ToString(@"hh\:mm") ?? "",
                    GercekCikis = kayit.CikisZamani?.ToString("HH:mm") ?? "",
                    ErkenCikisSuresi = kayit.ErkenCikisSuresi ?? 0
                });
            }

            return result.OrderByDescending(r => r.ErkenCikisSuresi).ToList();
        }

        public async Task<List<FazlaMesaiRaporDTO>> MesaiyeKalanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.FazlaMesaiSuresi > 0);

            var result = new List<FazlaMesaiRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new FazlaMesaiRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                    FazlaMesaiSuresi = kayit.FazlaMesaiSuresi ?? 0,
                    FazlaMesaiUcreti = CalculateOvertimePay(personel.Maas ?? 0m, kayit.FazlaMesaiSuresi ?? 0)  // ✅ DÜZELTILDI
                });
            }

            return result.OrderByDescending(r => r.FazlaMesaiSuresi).ToList();
        }

        public async Task<List<DevamsizlarRaporDTO>> DevamsizlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var aktifPersoneller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
            var result = new List<DevamsizlarRaporDTO>();

            foreach (var personel in aktifPersoneller)
            {
                var gunler = (bitis - baslangic).Days + 1;
                var devamsizGunler = new List<DateTime>();

                for (var tarih = baslangic; tarih <= bitis; tarih = tarih.AddDays(1))
                {
                    // Check if weekend
                    if (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    // Check if holiday
                    var tatil = await _unitOfWork.Tatiller.FirstOrDefaultAsync(t => t.Tarih.Date == tarih.Date);
                    if (tatil != null)
                        continue;

                    // Check if on leave
                    var izin = await _unitOfWork.Izinler.FirstOrDefaultAsync(i =>
                        i.PersonelId == personel.Id &&
                        i.BaslangicTarihi <= tarih &&
                        i.BitisTarihi >= tarih &&
                        i.OnayDurumu == "Onaylandı");
                    if (izin != null)
                        continue;

                    // Check attendance
                    var giris = await _unitOfWork.GirisCikislar.FirstOrDefaultAsync(g =>
                        g.PersonelId == personel.Id &&
                        g.GirisZamani.HasValue &&
                        g.GirisZamani.Value.Date == tarih.Date);

                    if (giris == null)
                        devamsizGunler.Add(tarih);
                }

                if (devamsizGunler.Any())
                {
                    result.Add(new DevamsizlarRaporDTO
                    {
                        PersonelAdi = personel.AdSoyad,
                        SicilNo = personel.SicilNo,
                        Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                        DevamsizGunSayisi = devamsizGunler.Count,
                        DevamsizGunler = string.Join(", ", devamsizGunler.Select(d => d.ToString("dd.MM.yyyy")))
                    });
                }
            }

            return result.OrderByDescending(r => r.DevamsizGunSayisi).ToList();
        }

        public async Task<MaasBordrosuDTO> KisiBazindaMaasBordrosu(int personelId, int yil, int ay)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null) return null;

            var donemBaslangic = new DateTime(yil, ay, 1);
            var donemBitis = donemBaslangic.AddMonths(1).AddDays(-1);

            // Calculate working days
            var calismaGunleri = await _unitOfWork.GirisCikislar.CountAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= donemBaslangic &&
                g.GirisZamani <= donemBitis);

            // Calculate overtime
            var fazlaMesailar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= donemBaslangic &&
                g.GirisZamani <= donemBitis &&
                g.FazlaMesaiSuresi > 0);

            var toplamFazlaMesai = fazlaMesailar.Sum(f => f.FazlaMesaiSuresi ?? 0);
            var fazlaMesaiUcreti = CalculateOvertimePay(personel.Maas ?? 0m, toplamFazlaMesai);  // ✅ DÜZELTILDI

            // Get bonuses - Donem string olarak tanımlandığı için DateTime parse etmiyoruz
            var primler = await _unitOfWork.Primler.FindAsync(p =>
                p.PersonelId == personelId &&
                p.Yil == yil &&
                p.Ay == ay);  // ✅ DÜZELTILDI: Year ve Month yerine Yil ve Ay

            var toplamPrim = primler.Sum(p => (decimal?)p.Tutar) ?? 0m;  // ✅ DÜZELTILDI

            // Get advances
            var avanslar = await _unitOfWork.Avanslar.FindAsync(a =>
                a.PersonelId == personelId &&
                a.OdemeTarihi.HasValue &&
                a.OdemeTarihi.Value >= donemBaslangic &&
                a.OdemeTarihi.Value <= donemBitis &&
                a.Durum == "Ödendi");

            var toplamAvans = avanslar.Sum(a => (decimal?)a.Tutar) ?? 0m;  // ✅ DÜZELTILDI

            var brutMaas = (personel.Maas ?? 0m) + fazlaMesaiUcreti + toplamPrim;
            var sgkKesintisi = brutMaas * 0.14m;
            var gelirVergisi = brutMaas * 0.15m;
            var netMaas = brutMaas - sgkKesintisi - gelirVergisi - toplamAvans;

            return new MaasBordrosuDTO
            {
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                Donem = $"{ay:00}/{yil}",
                BrutMaas = personel.Maas ?? 0m,
                FazlaMesaiUcreti = fazlaMesaiUcreti,
                Primler = toplamPrim,
                ToplamBrut = brutMaas,
                SGKKesintisi = sgkKesintisi,
                GelirVergisi = gelirVergisi,
                AvansKesintisi = toplamAvans,
                NetMaas = netMaas,
                CalismaGunSayisi = calismaGunleri,
                FazlaMesaiSaati = (int)(toplamFazlaMesai / 60)
            };
        }

        public async Task<List<MaasBordrosuDTO>> GenelBazdaMaasBordrosu(int yil, int ay)
        {
            var personeller = await _unitOfWork.Personeller.FindAsync(p => p.Durum);
            var result = new List<MaasBordrosuDTO>();

            foreach (var personel in personeller)
            {
                var bordro = await KisiBazindaMaasBordrosu(personel.Id, yil, ay);
                if (bordro != null)
                    result.Add(bordro);
            }

            return result.OrderBy(b => b.PersonelAdi).ToList();
        }

        // Helper methods
        private int CalculateWorkMinutes(DateTime? giris, DateTime? cikis)
        {
            if (!giris.HasValue || !cikis.HasValue)
                return 0;

            return (int)(cikis.Value - giris.Value).TotalMinutes;
        }

        private decimal CalculateOvertimePay(decimal maas, int fazlaMesaiDakika)
        {
            // Aylık 160 saat (4 hafta x 40 saat) standart çalışma
            var saatlikUcret = maas / 160m;
            var fazlaMesaiSaat = fazlaMesaiDakika / 60m;
            var fazlaMesaiOrani = 1.5m; // %150 ücret

            return saatlikUcret * fazlaMesaiSaat * fazlaMesaiOrani;
        }

        // Implement remaining report methods...
        public async Task<List<AvansListDTO>> AvansListesi(DateTime baslangic, DateTime bitis)
        {
            var avanslar = await _unitOfWork.Avanslar.FindAsync(a =>
                a.TalepTarihi >= baslangic &&
                a.TalepTarihi <= bitis);

            var result = new List<AvansListDTO>();
            foreach (var avans in avanslar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(avans.PersonelId);
                var onaylayan = avans.OnaylayanKullaniciId.HasValue ?  // ✅ DÜZELTILDI
                    await _unitOfWork.Kullanicilar.GetByIdAsync(avans.OnaylayanKullaniciId.Value) : null;

                result.Add(new AvansListDTO
                {
                    Id = avans.Id,
                    PersonelId = avans.PersonelId,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Tutar = avans.Tutar,
                    TalepTarihi = avans.TalepTarihi,
                    OdemeTarihi = avans.OdemeTarihi,
                    Durum = avans.Durum,
                    Aciklama = avans.Aciklama,
                    OnaylayanAdi = onaylayan?.Personel?.AdSoyad
                });
            }

            return result.OrderByDescending(a => a.TalepTarihi).ToList();
        }

        public async Task<MaasZarfiDTO> MaasZarfi(int personelId, int yil, int ay)
        {
            var bordro = await KisiBazindaMaasBordrosu(personelId, yil, ay);
            if (bordro == null) return null;

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);

            return new MaasZarfiDTO
            {
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                Gorev = personel.Gorev,
                Donem = bordro.Donem,
                BrutMaas = bordro.BrutMaas,
                FazlaMesaiUcreti = bordro.FazlaMesaiUcreti,
                Primler = bordro.Primler,
                ToplamBrut = bordro.ToplamBrut,
                SGKKesintisi = bordro.SGKKesintisi,
                GelirVergisi = bordro.GelirVergisi,
                AvansKesintisi = bordro.AvansKesintisi,
                NetMaas = bordro.NetMaas,
                OdemeYontemi = "Banka Havalesi",
                OdemeTarihi = DateTime.UtcNow
            };
        }

        public async Task<List<PrimListDTO>> PrimListesi(int yil, int ay)
        {
            var primler = await _unitOfWork.Primler.FindAsync(p =>
                p.Yil == yil && p.Ay == ay);  // ✅ DÜZELTILDI

            var result = new List<PrimListDTO>();
            foreach (var prim in primler)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(prim.PersonelId);
                result.Add(new PrimListDTO
                {
                    Id = prim.Id,
                    PersonelId = prim.PersonelId,
                    PersonelAdi = personel.AdSoyad,
                    Donem = prim.Donem,
                    Tutar = prim.Tutar,
                    PrimTipi = prim.PrimTipi,
                    Aciklama = prim.Aciklama
                });
            }

            return result.OrderByDescending(p => p.Tutar).ToList();
        }

        public async Task<List<IzinliPersonelRaporDTO>> IzinliPersonellerRaporu(DateTime baslangic, DateTime bitis)
        {
            var izinler = await _unitOfWork.Izinler.FindAsync(i =>
                i.OnayDurumu == "Onaylandı" &&
                ((i.BaslangicTarihi >= baslangic && i.BaslangicTarihi <= bitis) ||
                 (i.BitisTarihi >= baslangic && i.BitisTarihi <= bitis) ||
                 (i.BaslangicTarihi <= baslangic && i.BitisTarihi >= bitis)));

            var result = new List<IzinliPersonelRaporDTO>();
            foreach (var izin in izinler)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(izin.PersonelId);
                result.Add(new IzinliPersonelRaporDTO
                {
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  
                    IzinTipi = izin.IzinTipi,
                    BaslangicTarihi = izin.BaslangicTarihi,
                    BitisTarihi = izin.BitisTarihi,
                    GunSayisi = izin.IzinGunSayisi,
                    Aciklama = izin.Aciklama
                });
            }

            return result.OrderBy(r => r.BaslangicTarihi).ToList();
        }

        public async Task<List<TatilGunuCalisanlarRaporDTO>> TatilGunuCalisanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var tatiller = await _unitOfWork.Tatiller.FindAsync(t =>
                t.Tarih >= baslangic && t.Tarih <= bitis);

            var result = new List<TatilGunuCalisanlarRaporDTO>();

            foreach (var tatil in tatiller)
            {
                var calisanlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                    g.GirisZamani.HasValue &&
                    g.GirisZamani.Value.Date == tatil.Tarih.Date);

                foreach (var calisan in calisanlar)
                {
                    var personel = await _unitOfWork.Personeller.GetByIdAsync(calisan.PersonelId);
                    result.Add(new TatilGunuCalisanlarRaporDTO
                    {
                        TatilAdi = tatil.Ad,
                        Tarih = tatil.Tarih,
                        PersonelAdi = personel.AdSoyad,
                        SicilNo = personel.SicilNo,
                        Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                        GirisZamani = calisan.GirisZamani,
                        CikisZamani = calisan.CikisZamani,
                        CalismaSuresi = CalculateWorkMinutes(calisan.GirisZamani, calisan.CikisZamani)
                    });
                }
            }

            return result.OrderBy(r => r.Tarih).ToList();
        }

        public async Task<List<ElleGirisRaporDTO>> ElleGirisRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.ElleGiris);

            var result = new List<ElleGirisRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new ElleGirisRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  
                    GirisZamani = kayit.GirisZamani.GetValueOrDefault(),
                    CikisZamani = kayit.CikisZamani,
                    Not = kayit.Not,
                    OlusturmaTarihi = kayit.OlusturmaTarihi
                });
            }

            return result.OrderByDescending(r => r.Tarih).ToList();
        }

        public async Task<List<KartUnutanlarRaporDTO>> KartUnutanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            // Kart unutanlar = Giriş kaydı yok VEYA elle giriş yapılmış personeller
            return await ElleGirisRaporu(baslangic, bitis)
                .ContinueWith(t => t.Result.Select(e => new KartUnutanlarRaporDTO
                {
                    Tarih = e.Tarih,
                    PersonelAdi = e.PersonelAdi,
                    SicilNo = e.SicilNo,
                    Departman = e.Departman,
                    GirisZamani = e.GirisZamani,
                    CikisZamani = e.CikisZamani,
                    Not = e.Not
                }).ToList());
        }

        public async Task<List<IseGirenlerRaporDTO>> IseGirenlerRaporu(DateTime baslangic, DateTime bitis)
        {
            var yeniPersoneller = await _unitOfWork.Personeller.FindAsync(p =>
                p.GirisTarihi >= baslangic &&
                p.GirisTarihi <= bitis);

            return yeniPersoneller.Select(p => new IseGirenlerRaporDTO
            {
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad,  // ✅ DÜZELTILDI
                Gorev = p.Gorev,
                Email = p.Email,
                Telefon = p.Telefon,
                GirisTarihi = p.GirisTarihi,
                VardiyaAdi = p.Vardiya?.Ad
            }).OrderBy(r => r.GirisTarihi).ToList();
        }

        public async Task<List<IstenAyrilanlarRaporDTO>> IstenAyrilanlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var ayrilanlar = await _unitOfWork.Personeller.FindAsync(p =>
                p.CikisTarihi.HasValue &&
                p.CikisTarihi.Value >= baslangic &&
                p.CikisTarihi.Value <= bitis);

            return ayrilanlar.Select(p => new IstenAyrilanlarRaporDTO
            {
                AdSoyad = p.AdSoyad,
                SicilNo = p.SicilNo,
                Departman = p.Departman?.Ad,  // ✅ DÜZELTILDI
                Gorev = p.Gorev,
                GirisTarihi = p.GirisTarihi,
                CikisTarihi = p.CikisTarihi.Value,
                CalismaSuresi = (int)(p.CikisTarihi.Value - p.GirisTarihi).TotalDays
            }).OrderBy(r => r.CikisTarihi).ToList();
        }

        public async Task<List<NotluKayitlarRaporDTO>> NotluKayitlarRaporu(DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                !string.IsNullOrEmpty(g.Not));

            var result = new List<NotluKayitlarRaporDTO>();
            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                result.Add(new NotluKayitlarRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                    GirisZamani = kayit.GirisZamani,
                    CikisZamani = kayit.CikisZamani,
                    Not = kayit.Not,
                    OlusturmaTarihi = kayit.OlusturmaTarihi
                });
            }

            return result.OrderByDescending(r => r.Tarih).ToList();
        }

        public async Task<AylikDevamCizelgesiDTO> AylikDevamCizelgesi(int personelId, int yil, int ay)
        {
            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null) return null;

            var donemBaslangic = new DateTime(yil, ay, 1);
            var donemBitis = donemBaslangic.AddMonths(1).AddDays(-1);

            var gunler = new List<DevamGunDTO>();

            for (var tarih = donemBaslangic; tarih <= donemBitis; tarih = tarih.AddDays(1))
            {
                var giris = await _unitOfWork.GirisCikislar.FirstOrDefaultAsync(g =>
                    g.PersonelId == personelId &&
                    g.GirisZamani.HasValue &&
                    g.GirisZamani.Value.Date == tarih.Date);

                var izin = await _unitOfWork.Izinler.FirstOrDefaultAsync(i =>
                    i.PersonelId == personelId &&
                    i.BaslangicTarihi <= tarih &&
                    i.BitisTarihi >= tarih &&
                    i.OnayDurumu == "Onaylandı");

                var tatil = await _unitOfWork.Tatiller.FirstOrDefaultAsync(t => t.Tarih.Date == tarih.Date);

                string durum;
                if (tatil != null)
                    durum = "Tatil";
                else if (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                    durum = "Hafta Sonu";
                else if (izin != null)
                    durum = $"İzinli ({izin.IzinTipi})";
                else if (giris != null)
                    durum = giris.Durum;
                else
                    durum = "Devamsız";

                gunler.Add(new DevamGunDTO
                {
                    Tarih = tarih,
                    GirisZamani = giris?.GirisZamani,
                    CikisZamani = giris?.CikisZamani,
                    Durum = durum,
                    CalismaSuresi = giris != null ? CalculateWorkMinutes(giris.GirisZamani, giris.CikisZamani) : 0
                });
            }

            return new AylikDevamCizelgesiDTO
            {
                PersonelAdi = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                Departman = personel.Departman?.Ad,  // ✅ DÜZELTILDI
                Donem = $"{ay:00}/{yil}",
                Gunler = gunler,
                ToplamCalismaGunu = gunler.Count(g => g.Durum == "Normal" || g.Durum == "Geç Kalmış" || g.Durum == "Fazla Mesai"),
                ToplamDevamsizGun = gunler.Count(g => g.Durum == "Devamsız"),
                ToplamIzinGun = gunler.Count(g => g.Durum.Contains("İzinli")),
                ToplamCalismaSaati = (int)(gunler.Sum(g => g.CalismaSuresi) / 60)
            };
        }
    }
}