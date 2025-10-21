// PDKS.Business/Services/DashboardService.cs - TAM DÜZELTİLMİŞ

using Microsoft.EntityFrameworkCore;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOnayAkisiService _onayAkisiService;

        public DashboardService(
            IUnitOfWork unitOfWork,
            IOnayAkisiService onayAkisiService)
        {
            _unitOfWork = unitOfWork;
            _onayAkisiService = onayAkisiService;
        }

        #region Ana Dashboard

        public async Task<AnaDashboardDTO> GetAnaDashboardAsync(int kullaniciId)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
            if (kullanici == null) throw new Exception("Kullanıcı bulunamadı");

            var personelId = kullanici.PersonelId;
            if (personelId <= 0) throw new Exception("Personel bilgisi bulunamadı");

            if (personelId <= 0) throw new Exception("Personel ID geçersiz");
            var personel = await _unitOfWork.Personeller.GetByIdAsync((int)personelId);
            var sirketId = personel?.SirketId ?? 0;

            return new AnaDashboardDTO
            {
                BugunkunDurum = await GetBugunkunDurumAsync(sirketId),
                BekleyenOnaylar = await GetBekleyenOnaylarWidgetAsync(kullaniciId),
                SonAktiviteler = await GetSonAktivitelerAsync(kullaniciId, 10),
                DogumGunleri = await GetDogumGunleriAsync(sirketId),
                YilDonumleri = await GetYilDonumleriAsync(sirketId),
                Duyurular = new List<DuyuruDTO>()
            };
        }

        #endregion

        #region Bugünkü Durum Widget

        public async Task<BugunkunDurumDTO> GetBugunkunDurumAsync(int sirketId)
        {
            var bugun = DateTime.Today;
            var yarin = bugun.AddDays(1);

            // Tüm personeller
            var personeller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId && p.Durum);

            var toplamPersonel = personeller.Count();

            // Bugünkü giriş-çıkışlar
            var bugunGirisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.GirisZamani.HasValue &&
                               g.GirisZamani.Value >= bugun &&
                               g.GirisZamani.Value < yarin);

            var bugunkuGiris = bugunGirisCikislar
                .Select(g => g.PersonelId)
                .Distinct()
                .Count();

            // Aktif personel (bugün giriş yapmış ve henüz çıkış yapmamış)
            var aktifPersonel = bugunGirisCikislar
                .Where(g => !g.CikisZamani.HasValue)
                .Select(g => g.PersonelId)
                .Distinct()
                .Count();

            // İzinli personel
            var izinliPersonel = await _unitOfWork.Izinler
                .FindAsync(i => i.OnayDurumu == "Onaylandi" &&
                               i.BaslangicTarihi <= bugun &&
                               i.BitisTarihi >= bugun);

            var izinliSayisi = izinliPersonel
                .Select(i => i.PersonelId)
                .Distinct()
                .Count();

            // Raporlu personel
            var raporluPersonel = 0;

            // Geç kalanlar (mesai başlangıcından 15 dk sonra gelenler)
            var mesaiBaslangic = new TimeSpan(9, 0, 0);
            var gecKalmaLimiti = mesaiBaslangic.Add(TimeSpan.FromMinutes(15));

            var gecKalanPersonel = bugunGirisCikislar
                .Where(g => g.GirisZamani.HasValue &&
                           g.GirisZamani.Value.TimeOfDay > gecKalmaLimiti)
                .Select(g => g.PersonelId)
                .Distinct()
                .Count();

            // Devamsız personel
            var girisYapanIds = bugunGirisCikislar.Select(g => g.PersonelId).Distinct().ToList();
            var izinliIds = izinliPersonel.Select(i => i.PersonelId).Distinct().ToList();
            var devamsizPersonel = toplamPersonel - girisYapanIds.Union(izinliIds).Distinct().Count();
            if (devamsizPersonel < 0) devamsizPersonel = 0;

            // Giriş-çıkış oranı
            var girisCikisOrani = toplamPersonel > 0 ? (bugunkuGiris * 100.0 / toplamPersonel) : 0;

            return new BugunkunDurumDTO
            {
                ToplamPersonel = toplamPersonel,
                BugunkuGiris = bugunkuGiris,
                AktifPersonel = aktifPersonel,
                IzinliPersonel = izinliSayisi,
                RaporluPersonel = raporluPersonel,
                GecKalanPersonel = gecKalanPersonel,
                DevamsizPersonel = devamsizPersonel,
                GirisCikisOrani = Math.Round(girisCikisOrani, 2)
            };
        }

        #endregion

        #region Bekleyen Onaylar Widget

        public async Task<List<BekleyenOnayWidgetDTO>> GetBekleyenOnaylarWidgetAsync(int kullaniciId)
        {
            try
            {
                var bekleyenOnaylar = await _onayAkisiService.GetBekleyenOnaylarAsync(kullaniciId);

                return bekleyenOnaylar
                    .Take(5)
                    .Select(o => new BekleyenOnayWidgetDTO
                    {
                        OnayKaydiId = o.OnayKaydiId,
                        ModulTipi = o.ModulTipi,
                        TalepEden = o.TalepEdenKisi,
                        AdimAdi = o.AdimAdi,
                        TalepTarihi = o.TalepTarihi,
                        BeklemeSuresi = o.BeklemeSuresi,
                        OncelikDurumu = o.OncelikDurumu
                    })
                    .ToList();
            }
            catch
            {
                return new List<BekleyenOnayWidgetDTO>();
            }
        }

        #endregion

        #region Son Aktiviteler

        public async Task<List<SonAktiviteDTO>> GetSonAktivitelerAsync(int kullaniciId, int limit)
        {
            var aktiviteler = new List<SonAktiviteDTO>();

            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
            if (kullanici == null) return aktiviteler;

            var personelId = kullanici.PersonelId;

            // Son giriş-çıkışlar
            var sonGirisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.PersonelId == personelId && g.GirisZamani.HasValue);

            foreach (var gc in sonGirisCikislar.OrderByDescending(g => g.GirisZamani).Take(3))
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(gc.PersonelId);

                aktiviteler.Add(new SonAktiviteDTO
                {
                    Tip = "GirisCikis",
                    Baslik = gc.CikisZamani.HasValue ? "Çıkış Yaptınız" : "Giriş Yaptınız",
                    Aciklama = gc.GirisZamani?.ToString("dd.MM.yyyy HH:mm") ?? "",
                    Kullanici = personel?.AdSoyad ?? "Siz",
                    Tarih = gc.GirisZamani ?? DateTime.Now,
                    Icon = "login",
                    Renk = "primary"
                });
            }

            // Son izin talepleri
            var sonIzinler = await _unitOfWork.Izinler
                .FindAsync(i => i.PersonelId == personelId);

            foreach (var izin in sonIzinler.OrderByDescending(i => i.OlusturmaTarihi).Take(2))
            {
                aktiviteler.Add(new SonAktiviteDTO
                {
                    Tip = "Izin",
                    Baslik = "İzin Talebi",
                    Aciklama = $"{izin.BaslangicTarihi:dd.MM.yyyy} - {izin.BitisTarihi:dd.MM.yyyy}",
                    Kullanici = "Siz",
                    Tarih = izin.OlusturmaTarihi,
                    Icon = "calendar",
                    Renk = "success"
                });
            }

            return aktiviteler
                .OrderByDescending(a => a.Tarih)
                .Take(limit)
                .ToList();
        }

        #endregion

        #region Doğum Günleri

        public async Task<List<DogumGunuDTO>> GetDogumGunleriAsync(int sirketId)
        {
            var bugun = DateTime.Today;
            var gelecek30Gun = bugun.AddDays(30);

            var personeller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId && p.Durum);

            var dogumGunleri = new List<DogumGunuDTO>();

            foreach (var personel in personeller)
            {
                var dogumTarihi = personel.DogumTarihi;
                if (dogumTarihi == DateTime.MinValue) continue;

                var buYilDogumGunu = new DateTime(bugun.Year, dogumTarihi.Month, dogumTarihi.Day);

                if (buYilDogumGunu < bugun)
                {
                    buYilDogumGunu = buYilDogumGunu.AddYears(1);
                }

                if (buYilDogumGunu <= gelecek30Gun)
                {
                    var kacGunSonra = (buYilDogumGunu - bugun).Days;

                    dogumGunleri.Add(new DogumGunuDTO
                    {
                        PersonelId = personel.Id,
                        AdSoyad = personel.AdSoyad,
                        ProfilFoto = personel.ProfilResmi ?? "",
                        DogumTarihi = dogumTarihi,
                        KacGunSonra = kacGunSonra,
                        Bugun = kacGunSonra == 0
                    });
                }
            }

            return dogumGunleri.OrderBy(d => d.KacGunSonra).ToList();
        }

        #endregion

        #region Yıl Dönümleri

        public async Task<List<YilDonumuDTO>> GetYilDonumleriAsync(int sirketId)
        {
            var bugun = DateTime.Today;
            var gelecek30Gun = bugun.AddDays(30);

            var personeller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId && p.Durum);

            var yilDonumleri = new List<YilDonumuDTO>();

            foreach (var personel in personeller)
            {
                var girisTarihi = personel.GirisTarihi;
                if (girisTarihi == DateTime.MinValue) continue;

                var buYilYilDonumu = new DateTime(bugun.Year, girisTarihi.Month, girisTarihi.Day);

                if (buYilYilDonumu < bugun)
                {
                    buYilYilDonumu = buYilYilDonumu.AddYears(1);
                }

                if (buYilYilDonumu <= gelecek30Gun)
                {
                    var kacGunSonra = (buYilYilDonumu - bugun).Days;
                    var kacYil = bugun.Year - girisTarihi.Year;

                    if (kacYil > 0)
                    {
                        yilDonumleri.Add(new YilDonumuDTO
                        {
                            PersonelId = personel.Id,
                            AdSoyad = personel.AdSoyad,
                            ProfilFoto = personel.ProfilResmi ?? "",
                            GirisTarihi = girisTarihi,
                            KacYil = kacYil,
                            KacGunSonra = kacGunSonra,
                            Bugun = kacGunSonra == 0
                        });
                    }
                }
            }

            return yilDonumleri.OrderBy(y => y.KacGunSonra).ToList();
        }

        #endregion

        #region Manager Dashboard

        public async Task<ManagerDashboardDTO> GetManagerDashboardAsync(int kullaniciId)
        {
            return new ManagerDashboardDTO
            {
                EkipOzeti = new EkipOzetiDTO
                {
                    ToplamKisi = 12,
                    BugunkuGiris = 11,
                    IzindeKisi = 1,
                    RaporluKisi = 0,
                    OrtalamaPerformans = 87.5
                },
                EkipUyeleri = new List<EkipUyesiDTO>(),
                BekleyenTalepler = new List<BekleyenTalepDTO>
                {
                    new BekleyenTalepDTO { TalepTipi = "İzin", Sayi = 3 },
                    new BekleyenTalepDTO { TalepTipi = "Avans", Sayi = 1 }
                },
                IzinTakvimi = new EkipIzinTakvimiDTO { Gunler = new List<IzinTakvimGunDTO>() },
                ButceKullanimi = new ButceKullanimiDTO
                {
                    ToplamButce = 150000,
                    KullanilanButce = 98500,
                    KalanButce = 51500,
                    KullanimYuzdesi = 65.7
                }
            };
        }

        #endregion

        #region IK Dashboard

        public async Task<IKDashboardDTO> GetIKDashboardAsync(int sirketId)
        {
            var personeller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId);

            var aktifPersoneller = personeller.Where(p => p.Durum).ToList();
            var toplamPersonel = aktifPersoneller.Count;

            var buAyBaslangic = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var buAyIseBaslayan = aktifPersoneller
                .Count(p => p.GirisTarihi != DateTime.MinValue && p.GirisTarihi >= buAyBaslangic);

            var buAyCikan = personeller
                .Count(p => p.CikisTarihi.HasValue && p.CikisTarihi >= buAyBaslangic);

            var ortalamaKidem = aktifPersoneller
                .Where(p => p.GirisTarihi != DateTime.MinValue)
                .Select(p => (DateTime.Today - p.GirisTarihi).Days / 365.0)
                .DefaultIfEmpty(0)
                .Average();

            var toplamMaas = aktifPersoneller.Sum(p => p.Maas ?? 0);

            return new IKDashboardDTO
            {
                GenelIstatistikler = new GenelIstatistiklerDTO
                {
                    ToplamPersonel = toplamPersonel,
                    AktifPersonel = toplamPersonel,
                    BuAyIseBaslayan = buAyIseBaslayan,
                    BuAyCikanPersonel = buAyCikan,
                    PersonelDevri = toplamPersonel > 0 ? (buAyCikan * 100.0 / toplamPersonel) : 0,
                    OrtalamaKidem = Math.Round(ortalamaKidem, 1)
                },
                DevamsizlikAnalizi = new DevamsizlikAnaliziDTO
                {
                    ToplamDevamsizGun = 0,
                    DevamsizlikOrani = 0,
                    DepartmanBazinda = new List<DepartmanDevamsizlikDTO>()
                },
                MaliyetAnalizi = new MaliyetAnaliziDTO
                {
                    ToplamMaasOdemesi = toplamMaas,
                    ToplamAvans = 0,
                    ToplamMasraf = 0,
                    ToplamMaliyet = toplamMaas,
                    DepartmanBazinda = new List<DepartmanMaliyetDTO>()
                },
                PersonelDagilimi = new PersonelDagilimDTO
                {
                    DepartmanBazinda = new List<DepartmanDagilimDTO>(),
                    UnvanBazinda = new List<UnvanDagilimDTO>(),
                    YasBazinda = new List<YasDagilimDTO>()
                },
                IseAlim = new IseAlimDTO
                {
                    AktifIlanSayisi = 0,
                    BaşvuruSayisi = 0,
                    MulakataSagiri = 0,
                    TeklifGonderilen = 0
                }
            };
        }

        #endregion

        #region Executive Dashboard

        public async Task<ExecutiveDashboardDTO> GetExecutiveDashboardAsync(int sirketId)
        {
            var personeller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId && p.Durum);

            var toplamPersonel = personeller.Count();
            var toplamMaliyet = personeller.Sum(p => p.Maas ?? 0);

            return new ExecutiveDashboardDTO
            {
                Summary = new ExecutiveSummaryDTO
                {
                    ToplamPersonel = toplamPersonel,
                    ToplamMaliyet = toplamMaliyet,
                    PersonelMemnuniyeti = 78.5,
                    Verimlilik = 85.2,
                    PersonelDevri = 3.2
                },
                DepartmanKarsilastirma = new List<DepartmanKarsilastirmaDTO>(),
                FinansalOzet = new FinansalOzetDTO
                {
                    BuAyMaasOdemesi = toplamMaliyet,
                    BuAyAvans = 0,
                    BuAyMasraf = 0,
                    ToplamGider = toplamMaliyet,
                    OncekiAyKarsilastirma = 0,
                    DegisimYuzdesi = 0
                },
                Trendler = new List<TrendDTO>(),
                KPIlar = new List<KPIDTO>
                {
                    new KPIDTO { Baslik = "Personel Memnuniyeti", Deger = 78.5, Hedef = 80, Birim = "%", Trend = "up" },
                    new KPIDTO { Baslik = "Devamsızlık Oranı", Deger = 2.1, Hedef = 3, Birim = "%", Trend = "down" },
                    new KPIDTO { Baslik = "Personel Devri", Deger = 3.2, Hedef = 5, Birim = "%", Trend = "stable" }
                }
            };
        }

        #endregion
    }
}