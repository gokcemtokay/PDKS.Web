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
    public interface IPuantajService
    {
        Task<IEnumerable<PuantajListDTO>> GetAllAsync(int sirketId, int yil, int ay);
        Task<PuantajDetailDTO> GetByIdAsync(int id);
        Task<PuantajDetailDTO> GetByPersonelAsync(int personelId, int yil, int ay);
        Task<int> OlusturAsync(PuantajCreateDTO dto);
        Task<List<int>> TopluOlusturAsync(PuantajTopluOlusturDTO dto);
        Task YenidenHesaplaAsync(int puantajId);
        Task OnaylaAsync(PuantajOnayDTO dto);
        Task<PuantajIstatistikDTO> GetIstatistikAsync(int sirketId, int yil, int ay);
        Task<IEnumerable<GecKalanlarRaporDTO>> GetGecKalanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<ErkenCikanlarRaporDTO>> GetErkenCikanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<FazlaMesaiRaporDTO>> GetFazlaMesaiRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<DevamsizlikRaporDTO>> GetDevamsizlikRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis);
    }

    public class PuantajService : IPuantajService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PuantajService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PuantajListDTO>> GetAllAsync(int sirketId, int yil, int ay)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindWithIncludesAsync(
                    p => p.SirketId == sirketId && p.Yil == yil && p.Ay == ay,
                    p => p.Personel,
                    p => p.Personel.Departman
                );

            return puantajlar.Select(p => new PuantajListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                PersonelId = p.PersonelId,
                PersonelAdSoyad = p.Personel.AdSoyad,
                SicilNo = p.Personel.SicilNo,
                DepartmanAdi = p.Personel.Departman?.Ad ?? "-",
                Yil = p.Yil,
                Ay = p.Ay,
                ToplamCalisilanGun = p.ToplamCalisilanGun,
                ToplamCalismaSuresi = p.ToplamCalismaSuresi,
                FazlaMesaiSuresi = p.FazlaMesaiSuresi,
                DevamsizlikGunSayisi = p.DevamsizlikGunSayisi,
                IzinGunSayisi = p.IzinGunSayisi,
                Durum = p.Durum,
                Onaylandi = p.Onaylandi,
                OlusturmaTarihi = p.OlusturmaTarihi
            }).OrderBy(p => p.PersonelAdSoyad);
        }

        public async Task<PuantajDetailDTO> GetByIdAsync(int id)
        {
            var puantaj = await _unitOfWork.Puantajlar
                .FindWithIncludesAsync(
                    p => p.Id == id,
                    p => p.Personel,
                    p => p.Personel.Departman,
                    p => p.Personel.Vardiya,
                    p => p.OnaylayanKullanici,
                    p => p.PuantajDetaylari
                );

            var p = puantaj.FirstOrDefault();
            if (p == null) return null;

            return new PuantajDetailDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                PersonelId = p.PersonelId,
                PersonelAdSoyad = p.Personel.AdSoyad,
                SicilNo = p.Personel.SicilNo,
                DepartmanAdi = p.Personel.Departman?.Ad ?? "-",
                VardiyaAdi = p.Personel.Vardiya?.Ad ?? "-",
                Yil = p.Yil,
                Ay = p.Ay,
                ToplamCalismaSuresi = p.ToplamCalismaSuresi,
                NormalMesaiSuresi = p.NormalMesaiSuresi,
                FazlaMesaiSuresi = p.FazlaMesaiSuresi,
                GeceMesaiSuresi = p.GeceMesaiSuresi,
                HaftaTatiliCalismaSuresi = p.HaftaTatiliCalismaSuresi,
                ResmiTatilCalismaSuresi = p.ResmiTatilCalismaSuresi,
                ToplamCalisilanGun = p.ToplamCalisilanGun,
                GecKalmaGunSayisi = p.GecKalmaGunSayisi,
                ErkenCikisGunSayisi = p.ErkenCikisGunSayisi,
                DevamsizlikGunSayisi = p.DevamsizlikGunSayisi,
                IzinGunSayisi = p.IzinGunSayisi,
                HastaTatiliGunSayisi = p.HastaTatiliGunSayisi,
                MazeretliIzinGunSayisi = p.MazeretliIzinGunSayisi,
                UcretsizIzinGunSayisi = p.UcretsizIzinGunSayisi,
                HaftaTatiliGunSayisi = p.HaftaTatiliGunSayisi,
                ResmiTatilGunSayisi = p.ResmiTatilGunSayisi,
                ToplamGecKalmaSuresi = p.ToplamGecKalmaSuresi,
                ToplamErkenCikisSuresi = p.ToplamErkenCikisSuresi,
                ToplamEksikCalismaSuresi = p.ToplamEksikCalismaSuresi,
                Durum = p.Durum,
                Onaylandi = p.Onaylandi,
                OnaylayanKullaniciId = p.OnaylayanKullaniciId,
                OnaylayanAdSoyad = p.OnaylayanKullanici != null ? $"{p.OnaylayanKullanici.Ad} {p.OnaylayanKullanici.Soyad}" : null,
                OnayTarihi = p.OnayTarihi,
                Notlar = p.Notlar,
                OlusturmaTarihi = p.OlusturmaTarihi,
                GuncellemeTarihi = p.GuncellemeTarihi,
                Detaylar = p.PuantajDetaylari.Select(d => new PuantajDetayItemDTO
                {
                    Id = d.Id,
                    Tarih = d.Tarih,
                    GunTipi = d.GunTipi,
                    IlkGiris = d.IlkGiris,
                    SonCikis = d.SonCikis,
                    ToplamCalismaSuresi = d.ToplamCalismaSuresi,
                    VardiyaBaslangic = d.VardiyaBaslangic,
                    VardiyaBitis = d.VardiyaBitis,
                    PlanlananCalismaSuresi = d.PlanlananCalismaSuresi,
                    CalismaDurumu = d.CalismaDurumu,
                    GecKalmaSuresi = d.GecKalmaSuresi,
                    ErkenCikisSuresi = d.ErkenCikisSuresi,
                    FazlaMesaiSuresi = d.FazlaMesaiSuresi,
                    EksikCalismaSuresi = d.EksikCalismaSuresi,
                    IzinliMi = d.IzinliMi,
                    IzinTuru = d.IzinTuru,
                    ToplamMolaSuresi = d.ToplamMolaSuresi,
                    ElleGirildiMi = d.ElleGirildiMi,
                    Notlar = d.Notlar
                }).OrderBy(d => d.Tarih).ToList()
            };
        }

        public async Task<PuantajDetailDTO> GetByPersonelAsync(int personelId, int yil, int ay)
        {
            var puantaj = await _unitOfWork.Puantajlar
                .FindWithIncludesAsync(
                    p => p.PersonelId == personelId && p.Yil == yil && p.Ay == ay,
                    p => p.Personel,
                    p => p.Personel.Departman,
                    p => p.Personel.Vardiya,
                    p => p.OnaylayanKullanici,
                    p => p.PuantajDetaylari
                );

            var p = puantaj.FirstOrDefault();
            if (p == null) return null;

            return await GetByIdAsync(p.Id);
        }

        public async Task<int> OlusturAsync(PuantajCreateDTO dto)
        {
            // Mevcut puantaj kontrolü
            var mevcutPuantaj = await _unitOfWork.Puantajlar
                .FirstOrDefaultAsync(p => p.PersonelId == dto.PersonelId && p.Yil == dto.Yil && p.Ay == dto.Ay);

            if (mevcutPuantaj != null)
            {
                throw new Exception("Bu dönem için zaten puantaj kaydı mevcut");
            }

            // Personel bilgilerini al
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            // Yeni puantaj oluştur
            var puantaj = new Puantaj
            {
                SirketId = dto.SirketId,
                PersonelId = dto.PersonelId,
                Yil = dto.Yil,
                Ay = dto.Ay,
                Durum = "Taslak",
                Onaylandi = false,
                Notlar = dto.Notlar,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Puantajlar.AddAsync(puantaj);
            await _unitOfWork.SaveChangesAsync();

            // Puantajı hesapla
            await PuantajHesaplaAsync(puantaj.Id, personel, dto.Yil, dto.Ay);

            return puantaj.Id;
        }

        public async Task<List<int>> TopluOlusturAsync(PuantajTopluOlusturDTO dto)
        {
            var olusturulanIdler = new List<int>();

            // Personel listesini belirle
            IEnumerable<Personel> personeller;
            if (dto.TumPersoneller)
            {
                personeller = await _unitOfWork.Personeller
                    .FindAsync(p => p.SirketId == dto.SirketId && p.Durum == true);
            }
            else if (dto.DepartmanId.HasValue)
            {
                personeller = await _unitOfWork.Personeller
                    .FindAsync(p => p.SirketId == dto.SirketId && p.DepartmanId == dto.DepartmanId && p.Durum == true);
            }
            else
            {
                personeller = await _unitOfWork.Personeller
                    .FindAsync(p => dto.PersonelIdler.Contains(p.Id));
            }

            foreach (var personel in personeller)
            {
                try
                {
                    // Mevcut puantaj kontrolü
                    var mevcutPuantaj = await _unitOfWork.Puantajlar
                        .FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.Yil == dto.Yil && p.Ay == dto.Ay);

                    if (mevcutPuantaj != null)
                        continue;

                    var createDto = new PuantajCreateDTO
                    {
                        SirketId = dto.SirketId,
                        PersonelId = personel.Id,
                        Yil = dto.Yil,
                        Ay = dto.Ay
                    };

                    var puantajId = await OlusturAsync(createDto);
                    olusturulanIdler.Add(puantajId);
                }
                catch (Exception)
                {
                    // Hata durumunda devam et
                    continue;
                }
            }

            return olusturulanIdler;
        }

        public async Task YenidenHesaplaAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            if (puantaj.Onaylandi)
                throw new Exception("Onaylanmış puantaj yeniden hesaplanamaz");

            var personel = await _unitOfWork.Personeller.GetByIdAsync(puantaj.PersonelId);
            await PuantajHesaplaAsync(puantajId, personel, puantaj.Yil, puantaj.Ay);
        }

        private async Task PuantajHesaplaAsync(int puantajId, Personel personel, int yil, int ay)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);

            // Dönem tarih aralığını belirle
            var baslangicTarihi = new DateTime(yil, ay, 1);
            var bitisTarihi = baslangicTarihi.AddMonths(1).AddDays(-1);

            // Mevcut detayları sil
            var mevcutDetaylar = await _unitOfWork.PuantajDetaylar
                .FindAsync(d => d.PuantajId == puantajId);
            _unitOfWork.PuantajDetaylar.DeleteRange(mevcutDetaylar);
            await _unitOfWork.SaveChangesAsync();

            // Giriş-çıkış kayıtlarını al
            var girisCikislar = await _unitOfWork.GirisCikislar
                .FindAsync(g => g.PersonelId == personel.Id 
                    && g.GirisZamani.HasValue 
                    && g.GirisZamani.Value.Date >= baslangicTarihi 
                    && g.GirisZamani.Value.Date <= bitisTarihi);

            // İzinleri al
            var izinler = await _unitOfWork.Izinler
                .FindAsync(i => i.PersonelId == personel.Id 
                    && i.OnayDurumu == "Onaylandı"
                    && i.BaslangicTarihi <= bitisTarihi 
                    && i.BitisTarihi >= baslangicTarihi);

            // Tatil günlerini al
            var tatiller = await _unitOfWork.Tatiller
                .FindAsync(t => t.Tarih >= baslangicTarihi && t.Tarih <= bitisTarihi);

            // Vardiya bilgilerini al
            Vardiya vardiya = null;
            if (personel.VardiyaId.HasValue)
            {
                vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value);
            }

            // İstatistik değişkenleri
            int toplamCalismaSuresi = 0;
            int normalMesaiSuresi = 0;
            int fazlaMesaiSuresi = 0;
            int geceMesaiSuresi = 0;
            int haftaTatiliCalismaSuresi = 0;
            int resmiTatilCalismaSuresi = 0;
            int toplamCalisilanGun = 0;
            int gecKalmaGunSayisi = 0;
            int erkenCikisGunSayisi = 0;
            int devamsizlikGunSayisi = 0;
            int izinGunSayisi = 0;
            int haftaTatiliGunSayisi = 0;
            int resmiTatilGunSayisi = 0;
            int toplamGecKalmaSuresi = 0;
            int toplamErkenCikisSuresi = 0;
            int toplamEksikCalismaSuresi = 0;

            // Ay içindeki her gün için detay oluştur
            for (var tarih = baslangicTarihi; tarih <= bitisTarihi; tarih = tarih.AddDays(1))
            {
                var detay = new PuantajDetay
                {
                    PuantajId = puantajId,
                    Tarih = tarih,
                    VardiyaId = personel.VardiyaId,
                    VardiyaBaslangic = vardiya?.BaslangicSaati,
                    VardiyaBitis = vardiya?.BitisSaati,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                // Gün tipini belirle
                var tatil = tatiller.FirstOrDefault(t => t.Tarih.Date == tarih.Date);
                if (tatil != null)
                {
                    detay.GunTipi = 3; // Resmi Tatil
                    resmiTatilGunSayisi++;
                }
                else if (tarih.DayOfWeek == DayOfWeek.Sunday || tarih.DayOfWeek == DayOfWeek.Saturday)
                {
                    detay.GunTipi = 2; // Hafta Tatili
                    haftaTatiliGunSayisi++;
                }
                else
                {
                    detay.GunTipi = 1; // Hafta İçi
                }

                // İzin kontrolü
                var izin = izinler.FirstOrDefault(i => i.BaslangicTarihi.Date <= tarih.Date && i.BitisTarihi.Date >= tarih.Date);
                if (izin != null)
                {
                    detay.IzinliMi = true;
                    detay.IzinId = izin.Id;
                    detay.IzinTuru = izin.IzinTipi;
                    detay.CalismaDurumu = "Izinli";
                    izinGunSayisi++;
                }
                else
                {
                    // Giriş-çıkış kontrolü
                    var gunGirisCikislar = girisCikislar.Where(g => g.GirisZamani.Value.Date == tarih.Date).OrderBy(g => g.GirisZamani).ToList();

                    if (gunGirisCikislar.Any())
                    {
                        detay.IlkGiris = gunGirisCikislar.First().GirisZamani;
                        detay.SonCikis = gunGirisCikislar.Last().CikisZamani;

                        // Toplam çalışma süresini hesapla
                        if (detay.SonCikis.HasValue)
                        {
                            detay.ToplamCalismaSuresi = (int)(detay.SonCikis.Value - detay.IlkGiris.Value).TotalMinutes;
                            toplamCalismaSuresi += detay.ToplamCalismaSuresi.Value;
                            toplamCalisilanGun++;
                        }

                        // Vardiya bazlı değerlendirme
                        if (vardiya != null)
                        {
                            var vardiyaBaslangicZaman = tarih.Date.Add(vardiya.BaslangicSaati);
                            var vardiyaBitisZaman = tarih.Date.Add(vardiya.BitisSaati);

                            // Gece vardiyası kontrolü
                            if (vardiya.GeceVardiyasiMi && vardiya.BitisSaati < vardiya.BaslangicSaati)
                            {
                                vardiyaBitisZaman = vardiyaBitisZaman.AddDays(1);
                            }

                            var planlananSure = (int)(vardiyaBitisZaman - vardiyaBaslangicZaman).TotalMinutes;
                            detay.PlanlananCalismaSuresi = planlananSure;

                            // Geç kalma kontrolü
                            var toleransDakika = vardiya.ToleransSuresiDakika;
                            if (detay.IlkGiris > vardiyaBaslangicZaman.AddMinutes(toleransDakika))
                            {
                                detay.GecKalmaSuresi = (int)(detay.IlkGiris.Value - vardiyaBaslangicZaman).TotalMinutes;
                                toplamGecKalmaSuresi += detay.GecKalmaSuresi.Value;
                                gecKalmaGunSayisi++;
                            }

                            // Erken çıkış kontrolü
                            if (detay.SonCikis.HasValue && detay.SonCikis < vardiyaBitisZaman.AddMinutes(-toleransDakika))
                            {
                                detay.ErkenCikisSuresi = (int)(vardiyaBitisZaman - detay.SonCikis.Value).TotalMinutes;
                                toplamErkenCikisSuresi += detay.ErkenCikisSuresi.Value;
                                erkenCikisGunSayisi++;
                            }

                            // Fazla mesai kontrolü
                            if (detay.SonCikis.HasValue && detay.SonCikis > vardiyaBitisZaman)
                            {
                                detay.FazlaMesaiSuresi = (int)(detay.SonCikis.Value - vardiyaBitisZaman).TotalMinutes;
                                fazlaMesaiSuresi += detay.FazlaMesaiSuresi.Value;

                                // Hafta tatili veya resmi tatil fazla mesai
                                if (detay.GunTipi == 2)
                                    haftaTatiliCalismaSuresi += detay.ToplamCalismaSuresi.Value;
                                else if (detay.GunTipi == 3)
                                    resmiTatilCalismaSuresi += detay.ToplamCalismaSuresi.Value;
                            }

                            // Eksik çalışma kontrolü
                            if (detay.ToplamCalismaSuresi < planlananSure)
                            {
                                detay.EksikCalismaSuresi = planlananSure - detay.ToplamCalismaSuresi.Value;
                                toplamEksikCalismaSuresi += detay.EksikCalismaSuresi.Value;
                            }

                            // Normal mesai hesapla
                            var normalSure = Math.Min(detay.ToplamCalismaSuresi.Value, planlananSure);
                            normalMesaiSuresi += normalSure;

                            // Çalışma durumunu belirle
                            if (detay.GecKalmaSuresi > 0 && detay.ErkenCikisSuresi > 0)
                                detay.CalismaDurumu = "GecKalmisErkenCikmis";
                            else if (detay.GecKalmaSuresi > 0)
                                detay.CalismaDurumu = "GecKalmis";
                            else if (detay.ErkenCikisSuresi > 0)
                                detay.CalismaDurumu = "ErkenCikmis";
                            else if (detay.FazlaMesaiSuresi > 0)
                                detay.CalismaDurumu = "FazlaMesai";
                            else
                                detay.CalismaDurumu = "Normal";
                        }
                    }
                    else if (detay.GunTipi == 1) // Hafta içi ve çalışmamış
                    {
                        detay.CalismaDurumu = "Devamsiz";
                        devamsizlikGunSayisi++;
                    }
                    else
                    {
                        detay.CalismaDurumu = detay.GunTipi == 2 ? "HaftaTatili" : "ResmiTatil";
                    }
                }

                await _unitOfWork.PuantajDetaylar.AddAsync(detay);
            }

            // Puantaj özetini güncelle
            puantaj.ToplamCalismaSuresi = toplamCalismaSuresi;
            puantaj.NormalMesaiSuresi = normalMesaiSuresi;
            puantaj.FazlaMesaiSuresi = fazlaMesaiSuresi;
            puantaj.GeceMesaiSuresi = geceMesaiSuresi;
            puantaj.HaftaTatiliCalismaSuresi = haftaTatiliCalismaSuresi;
            puantaj.ResmiTatilCalismaSuresi = resmiTatilCalismaSuresi;
            puantaj.ToplamCalisilanGun = toplamCalisilanGun;
            puantaj.GecKalmaGunSayisi = gecKalmaGunSayisi;
            puantaj.ErkenCikisGunSayisi = erkenCikisGunSayisi;
            puantaj.DevamsizlikGunSayisi = devamsizlikGunSayisi;
            puantaj.IzinGunSayisi = izinGunSayisi;
            puantaj.HaftaTatiliGunSayisi = haftaTatiliGunSayisi;
            puantaj.ResmiTatilGunSayisi = resmiTatilGunSayisi;
            puantaj.ToplamGecKalmaSuresi = toplamGecKalmaSuresi;
            puantaj.ToplamErkenCikisSuresi = toplamErkenCikisSuresi;
            puantaj.ToplamEksikCalismaSuresi = toplamEksikCalismaSuresi;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task OnaylaAsync(PuantajOnayDTO dto)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(dto.PuantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            puantaj.Onaylandi = dto.Onayla;
            puantaj.Durum = dto.Onayla ? "Onaylandi" : "Taslak";
            puantaj.OnayTarihi = dto.Onayla ? DateTime.UtcNow : null;
            // OnaylayanKullaniciId JWT'den alınmalı
            puantaj.Notlar = dto.Notlar;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PuantajIstatistikDTO> GetIstatistikAsync(int sirketId, int yil, int ay)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.SirketId == sirketId && p.Yil == yil && p.Ay == ay);

            var aktifPersoneller = await _unitOfWork.Personeller
                .FindAsync(p => p.SirketId == sirketId && p.Durum == true);

            return new PuantajIstatistikDTO
            {
                SirketId = sirketId,
                Yil = yil,
                Ay = ay,
                ToplamPersonelSayisi = aktifPersoneller.Count(),
                PuantajHesaplananSayisi = puantajlar.Count(),
                PuantajOnaylananSayisi = puantajlar.Count(p => p.Onaylandi),
                ToplamCalismaSuresi = puantajlar.Sum(p => p.ToplamCalismaSuresi),
                ToplamFazlaMesaiSuresi = puantajlar.Sum(p => p.FazlaMesaiSuresi),
                ToplamDevamsizlikGunSayisi = puantajlar.Sum(p => p.DevamsizlikGunSayisi),
                ToplamIzinGunSayisi = puantajlar.Sum(p => p.IzinGunSayisi)
            };
        }

        public async Task<IEnumerable<GecKalanlarRaporDTO>> GetGecKalanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var puantajDetaylar = await _unitOfWork.PuantajDetaylar
                .FindWithIncludesAsync(
                    d => d.Puantaj.SirketId == sirketId 
                        && d.Tarih >= baslangic 
                        && d.Tarih <= bitis 
                        && d.GecKalmaSuresi > 0,
                    d => d.Puantaj,
                    d => d.Puantaj.Personel,
                    d => d.Puantaj.Personel.Departman
                );

            return puantajDetaylar.Select(d => new GecKalanlarRaporDTO
            {
                Tarih = d.Tarih,
                PersonelId = d.Puantaj.PersonelId,
                PersonelAdSoyad = d.Puantaj.Personel.AdSoyad,
                SicilNo = d.Puantaj.Personel.SicilNo,
                DepartmanAdi = d.Puantaj.Personel.Departman?.Ad ?? "-",
                VardiyaBaslangic = d.VardiyaBaslangic ?? TimeSpan.Zero,
                GirisSaati = d.IlkGiris,
                GecKalmaSuresi = d.GecKalmaSuresi ?? 0
            }).OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdSoyad);
        }

        public async Task<IEnumerable<ErkenCikanlarRaporDTO>> GetErkenCikanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var puantajDetaylar = await _unitOfWork.PuantajDetaylar
                .FindWithIncludesAsync(
                    d => d.Puantaj.SirketId == sirketId 
                        && d.Tarih >= baslangic 
                        && d.Tarih <= bitis 
                        && d.ErkenCikisSuresi > 0,
                    d => d.Puantaj,
                    d => d.Puantaj.Personel,
                    d => d.Puantaj.Personel.Departman
                );

            return puantajDetaylar.Select(d => new ErkenCikanlarRaporDTO
            {
                Tarih = d.Tarih,
                PersonelId = d.Puantaj.PersonelId,
                PersonelAdSoyad = d.Puantaj.Personel.AdSoyad,
                SicilNo = d.Puantaj.Personel.SicilNo,
                DepartmanAdi = d.Puantaj.Personel.Departman?.Ad ?? "-",
                VardiyaBitis = d.VardiyaBitis ?? TimeSpan.Zero,
                CikisSaati = d.SonCikis,
                ErkenCikisSuresi = d.ErkenCikisSuresi ?? 0
            }).OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdSoyad);
        }

        public async Task<IEnumerable<FazlaMesaiRaporDTO>> GetFazlaMesaiRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var puantajDetaylar = await _unitOfWork.PuantajDetaylar
                .FindWithIncludesAsync(
                    d => d.Puantaj.SirketId == sirketId 
                        && d.Tarih >= baslangic 
                        && d.Tarih <= bitis 
                        && d.FazlaMesaiSuresi > 0,
                    d => d.Puantaj,
                    d => d.Puantaj.Personel,
                    d => d.Puantaj.Personel.Departman
                );

            return puantajDetaylar.Select(d => new FazlaMesaiRaporDTO
            {
                Tarih = d.Tarih,
                PersonelId = d.Puantaj.PersonelId,
                PersonelAdSoyad = d.Puantaj.Personel.AdSoyad,
                SicilNo = d.Puantaj.Personel.SicilNo,
                DepartmanAdi = d.Puantaj.Personel.Departman?.Ad ?? "-",
                FazlaMesaiSuresi = d.FazlaMesaiSuresi ?? 0,
                HaftaTatiliMi = d.GunTipi == 2,
                ResmiTatilMi = d.GunTipi == 3
            }).OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdSoyad);
        }

        public async Task<IEnumerable<DevamsizlikRaporDTO>> GetDevamsizlikRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var puantajDetaylar = await _unitOfWork.PuantajDetaylar
                .FindWithIncludesAsync(
                    d => d.Puantaj.SirketId == sirketId 
                        && d.Tarih >= baslangic 
                        && d.Tarih <= bitis 
                        && d.CalismaDurumu == "Devamsiz",
                    d => d.Puantaj,
                    d => d.Puantaj.Personel,
                    d => d.Puantaj.Personel.Departman
                );

            return puantajDetaylar.Select(d => new DevamsizlikRaporDTO
            {
                Tarih = d.Tarih,
                PersonelId = d.Puantaj.PersonelId,
                PersonelAdSoyad = d.Puantaj.Personel.AdSoyad,
                SicilNo = d.Puantaj.Personel.SicilNo,
                DepartmanAdi = d.Puantaj.Personel.Departman?.Ad ?? "-",
                DevamsizlikNedeni = d.Notlar ?? "Belirtilmemiş"
            }).OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdSoyad);
        }
    }
}
