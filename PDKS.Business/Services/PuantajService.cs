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
    // CLASS İÇİNDE INTERFACE TANIMLAMASI KALDIRILDI - AYRI DOSYADA ZATEN VAR
    public class PuantajService : IPuantajService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PuantajService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ============================================
        // INTERFACE METODLARI - IPuantajService.cs'den gelen
        // ============================================

        // Puantaj Hesaplama
        public async Task<int> HesaplaPuantajAsync(PuantajHesaplaDTO dto)
        {
            // Mevcut puantaj kontrolü
            var mevcutPuantaj = await _unitOfWork.Puantajlar
                .FirstOrDefaultAsync(p => p.PersonelId == dto.PersonelId && p.Yil == dto.Yil && p.Ay == dto.Ay);

            if (mevcutPuantaj != null && !dto.YenidenHesapla)
                throw new Exception("Bu dönem için puantaj zaten mevcut");

            if (mevcutPuantaj != null && dto.YenidenHesapla)
            {
                await YenidenHesaplaAsync(mevcutPuantaj.Id);
                return mevcutPuantaj.Id;
            }

            // Yeni puantaj oluştur
            var personel = await _unitOfWork.Personeller.GetByIdAsync(dto.PersonelId);
            if (personel == null)
                throw new Exception("Personel bulunamadı");

            var puantaj = new Puantaj
            {
                SirketId = personel.SirketId,
                PersonelId = dto.PersonelId,
                Yil = dto.Yil,
                Ay = dto.Ay,
                Durum = "Taslak",
                Onaylandi = false,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Puantajlar.AddAsync(puantaj);
            await _unitOfWork.SaveChangesAsync();

            // Puantaj hesaplama işlemi
            await PuantajHesaplaAsync(puantaj.Id, personel, dto.Yil, dto.Ay);

            return puantaj.Id;
        }

        public async Task<List<int>> TopluPuantajHesaplaAsync(TopluPuantajHesaplaDTO dto)
        {
            var personeller = new List<Personel>();

            if (dto.TumPersonel)
            {
                var tumPersoneller = await _unitOfWork.Personeller.FindAsync(p => p.SirketId == dto.SirketId && p.Durum == true);
                personeller = tumPersoneller.ToList();
            }
            else if (dto.DepartmanId.HasValue)
            {
                var departmanPersonelleri = await _unitOfWork.Personeller
                    .FindAsync(p => p.DepartmanId == dto.DepartmanId.Value && p.Durum == true);
                personeller = departmanPersonelleri.ToList();
            }
            else if (dto.PersonelIdler != null && dto.PersonelIdler.Any())
            {
                foreach (var personelId in dto.PersonelIdler)
                {
                    var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
                    if (personel != null && personel.Durum == true)
                        personeller.Add(personel);
                }
            }

            var olusturulanIdler = new List<int>();

            foreach (var personel in personeller)
            {
                try
                {
                    var mevcutPuantaj = await _unitOfWork.Puantajlar
                        .FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.Yil == dto.Yil && p.Ay == dto.Ay);

                    if (mevcutPuantaj != null)
                        continue;

                    var hesaplaDto = new PuantajHesaplaDTO
                    {
                        PersonelId = personel.Id,
                        Yil = dto.Yil,
                        Ay = dto.Ay
                    };

                    var puantajId = await HesaplaPuantajAsync(hesaplaDto);
                    olusturulanIdler.Add(puantajId);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return olusturulanIdler;
        }

        public async Task<PuantajDetailDTO> YenidenHesaplaAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            if (puantaj.Onaylandi)
                throw new Exception("Onaylanmış puantaj yeniden hesaplanamaz");

            var personel = await _unitOfWork.Personeller.GetByIdAsync(puantaj.PersonelId);
            await PuantajHesaplaAsync(puantajId, personel, puantaj.Yil, puantaj.Ay);

            return await GetByIdAsync(puantajId);
        }

        // CRUD İşlemleri
        public async Task<PuantajDetailDTO> GetByIdAsync(int id)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.Id == id);

            var puantaj = puantajlar.FirstOrDefault();
            if (puantaj == null) return null;

            // İlişkili verileri manuel yükle
            var personel = await _unitOfWork.Personeller.GetByIdAsync(puantaj.PersonelId);
            var departman = personel?.DepartmanId != null
                ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                : null;
            var vardiya = personel?.VardiyaId != null
                ? await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value)
                : null;
            var onaylayan = puantaj.OnaylayanKullaniciId != null
                ? await _unitOfWork.Kullanicilar.GetByIdAsync(puantaj.OnaylayanKullaniciId.Value)
                : null;

            // Detayları getir
            var detaylar = await _unitOfWork.PuantajDetaylar.FindAsync(d => d.PuantajId == puantaj.Id);

            return new PuantajDetailDTO
            {
                Id = puantaj.Id,
                SirketId = puantaj.SirketId,
                PersonelId = puantaj.PersonelId,
                PersonelAdSoyad = personel?.AdSoyad ?? "-",
                SicilNo = personel?.SicilNo ?? "-",
                DepartmanAdi = departman?.Ad ?? "-",
                VardiyaAdi = vardiya?.Ad ?? "-",
                Yil = puantaj.Yil,
                Ay = puantaj.Ay,
                ToplamCalismaSuresi = puantaj.ToplamCalismaSuresi,
                NormalMesaiSuresi = puantaj.NormalMesaiSuresi,
                FazlaMesaiSuresi = puantaj.FazlaMesaiSuresi,
                GeceMesaiSuresi = puantaj.GeceMesaiSuresi,
                HaftaTatiliCalismaSuresi = puantaj.HaftaTatiliCalismaSuresi,
                ResmiTatilCalismaSuresi = puantaj.ResmiTatilCalismaSuresi,
                ToplamCalisilanGun = puantaj.ToplamCalisilanGun,
                GecKalmaGunSayisi = puantaj.GecKalmaGunSayisi,
                ErkenCikisGunSayisi = puantaj.ErkenCikisGunSayisi,
                DevamsizlikGunSayisi = puantaj.DevamsizlikGunSayisi,
                IzinGunSayisi = puantaj.IzinGunSayisi,
                HastaTatiliGunSayisi = puantaj.HastaTatiliGunSayisi,
                MazeretliIzinGunSayisi = puantaj.MazeretliIzinGunSayisi,
                UcretsizIzinGunSayisi = puantaj.UcretsizIzinGunSayisi,
                HaftaTatiliGunSayisi = puantaj.HaftaTatiliGunSayisi,
                ResmiTatilGunSayisi = puantaj.ResmiTatilGunSayisi,
                ToplamGecKalmaSuresi = puantaj.ToplamGecKalmaSuresi,
                ToplamErkenCikisSuresi = puantaj.ToplamErkenCikisSuresi,
                ToplamEksikCalismaSuresi = puantaj.ToplamEksikCalismaSuresi,
                Durum = puantaj.Durum,
                Onaylandi = puantaj.Onaylandi,
                OnaylayanKullaniciId = puantaj.OnaylayanKullaniciId,
                OnaylayanAdSoyad = onaylayan != null ? $"{onaylayan.Ad} {onaylayan.Soyad}" : null,
                OnayTarihi = puantaj.OnayTarihi,
                Notlar = puantaj.Notlar,
                OlusturmaTarihi = puantaj.OlusturmaTarihi,
                GuncellemeTarihi = puantaj.GuncellemeTarihi,
                Detaylar = detaylar.Select(d => new PuantajDetayItemDTO
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

        public async Task<PuantajDetailDTO> GetByPersonelVeDonemAsync(int personelId, int yil, int ay)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == personelId && p.Yil == yil && p.Ay == ay);

            var puantaj = puantajlar.FirstOrDefault();
            if (puantaj == null) return null;

            return await GetByIdAsync(puantaj.Id);
        }

        public async Task<IEnumerable<PuantajListDTO>> GetByDonemAsync(int yil, int ay, int? departmanId = null)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.Yil == yil && p.Ay == ay);

            var liste = new List<PuantajListDTO>();

            foreach (var p in puantajlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(p.PersonelId);
                if (personel == null) continue;

                if (departmanId.HasValue && personel.DepartmanId != departmanId.Value)
                    continue;

                var departman = personel.DepartmanId != null
                    ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                    : null;

                liste.Add(new PuantajListDTO
                {
                    Id = p.Id,
                    SirketId = p.SirketId,
                    PersonelId = p.PersonelId,
                    PersonelAdSoyad = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    DepartmanAdi = departman?.Ad ?? "-",
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
                });
            }

            return liste.OrderBy(p => p.PersonelAdSoyad);
        }

        public async Task<IEnumerable<PuantajListDTO>> GetByPersonelAsync(int personelId)
        {
            var puantajlar = await _unitOfWork.Puantajlar
                .FindAsync(p => p.PersonelId == personelId);

            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null) return new List<PuantajListDTO>();

            var departman = personel.DepartmanId != null
                ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                : null;

            return puantajlar.Select(p => new PuantajListDTO
            {
                Id = p.Id,
                SirketId = p.SirketId,
                PersonelId = p.PersonelId,
                PersonelAdSoyad = personel.AdSoyad,
                SicilNo = personel.SicilNo,
                DepartmanAdi = departman?.Ad ?? "-",
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
            }).OrderByDescending(p => p.Yil).ThenByDescending(p => p.Ay);
        }

        public async Task<bool> OnaylaAsync(PuantajOnayDTO dto)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(dto.PuantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            if (puantaj.Onaylandi)
                throw new Exception("Puantaj zaten onaylanmış");

            puantaj.Onaylandi = true;
            puantaj.Durum = "Onaylandı";
            puantaj.OnayTarihi = DateTime.UtcNow;
            // OnaylayanKullaniciId JWT'den alınmalı - şimdilik dto'dan
            puantaj.Notlar = dto.Notlar;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> OnayIptalAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            if (!puantaj.Onaylandi)
                throw new Exception("Puantaj zaten onaysız durumda");

            puantaj.Onaylandi = false;
            puantaj.Durum = "Taslak";
            puantaj.OnayTarihi = null;
            puantaj.OnaylayanKullaniciId = null;
            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(id);
            if (puantaj == null)
                return false;

            if (puantaj.Onaylandi)
                throw new Exception("Onaylanmış puantaj silinemez");

            // Detayları sil - DeleteRange yerine foreach kullan
            var detaylar = await _unitOfWork.PuantajDetaylar.FindAsync(d => d.PuantajId == id);
            foreach (var detay in detaylar)
            {
                _unitOfWork.PuantajDetaylar.Remove(detay);
            }

            _unitOfWork.Puantajlar.Remove(puantaj);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Günlük Detaylar
        public async Task<List<PuantajDetayDTO>> GetGunlukDetaylarAsync(int puantajId)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);
            if (puantaj == null)
                throw new Exception("Puantaj bulunamadı");

            var detaylar = await _unitOfWork.PuantajDetaylar.FindAsync(d => d.PuantajId == puantajId);

            var liste = new List<PuantajDetayDTO>();
            foreach (var d in detaylar)
            {
                var vardiya = d.VardiyaId != null ? await _unitOfWork.Vardiyalar.GetByIdAsync(d.VardiyaId.Value) : null;
                var izin = d.IzinId != null ? await _unitOfWork.Izinler.GetByIdAsync(d.IzinId.Value) : null;

                liste.Add(new PuantajDetayDTO
                {
                    Id = d.Id,
                    PuantajId = d.PuantajId,
                    Tarih = d.Tarih,
                    VardiyaAdi = vardiya?.Ad,
                    PlanlananGirisSaati = d.VardiyaBaslangic,
                    PlanlananCikisSaati = d.VardiyaBitis,
                    GerceklesenGirisSaati = d.IlkGiris,
                    GerceklesenCikisSaati = d.SonCikis,
                    ToplamCalismaDakika = d.ToplamCalismaSuresi,
                    NormalMesaiDakika = d.PlanlananCalismaSuresi,
                    FazlaMesaiDakika = d.FazlaMesaiSuresi,
                    GecKalmaDakika = d.GecKalmaSuresi,
                    ErkenCikisDakika = d.ErkenCikisSuresi,
                    Durum = d.CalismaDurumu,
                    IzinTuru = izin?.IzinTipi,
                    HaftaSonuMu = d.GunTipi == 2,
                    ResmiTatilMi = d.GunTipi == 3,
                    Notlar = d.Notlar
                });
            }

            return liste.OrderBy(d => d.Tarih).ToList();
        }

        public async Task<PuantajDetayDTO> GetDetayByTarihAsync(int personelId, DateTime tarih)
        {
            var puantajlar = await _unitOfWork.Puantajlar.FindAsync(p =>
                p.PersonelId == personelId &&
                p.Yil == tarih.Year &&
                p.Ay == tarih.Month);

            var puantaj = puantajlar.FirstOrDefault();
            if (puantaj == null)
                return null;

            var detaylar = await _unitOfWork.PuantajDetaylar.FindAsync(d =>
                d.PuantajId == puantaj.Id &&
                d.Tarih.Date == tarih.Date);

            var detay = detaylar.FirstOrDefault();
            if (detay == null)
                return null;

            var vardiya = detay.VardiyaId != null ? await _unitOfWork.Vardiyalar.GetByIdAsync(detay.VardiyaId.Value) : null;
            var izin = detay.IzinId != null ? await _unitOfWork.Izinler.GetByIdAsync(detay.IzinId.Value) : null;

            return new PuantajDetayDTO
            {
                Id = detay.Id,
                PuantajId = detay.PuantajId,
                Tarih = detay.Tarih,
                VardiyaAdi = vardiya?.Ad,
                PlanlananGirisSaati = detay.VardiyaBaslangic,
                PlanlananCikisSaati = detay.VardiyaBitis,
                GerceklesenGirisSaati = detay.IlkGiris,
                GerceklesenCikisSaati = detay.SonCikis,
                ToplamCalismaDakika = detay.ToplamCalismaSuresi,
                NormalMesaiDakika = detay.PlanlananCalismaSuresi,
                FazlaMesaiDakika = detay.FazlaMesaiSuresi,
                GecKalmaDakika = detay.GecKalmaSuresi,
                ErkenCikisDakika = detay.ErkenCikisSuresi,
                Durum = detay.CalismaDurumu,
                IzinTuru = izin?.IzinTipi,
                HaftaSonuMu = detay.GunTipi == 2,
                ResmiTatilMi = detay.GunTipi == 3,
                Notlar = detay.Notlar
            };
        }

        // Raporlama
        public async Task<PuantajOzetRaporDTO> GetOzetRaporAsync(PuantajRaporParametreDTO parametre)
        {
            // Basit implementasyon - detaylı rapor için genişletilebilir
            var puantajlar = await _unitOfWork.Puantajlar.FindAsync(p =>
                p.Yil >= parametre.BaslangicYil &&
                p.Yil <= parametre.BitisYil);

            if (parametre.PersonelId.HasValue)
            {
                puantajlar = puantajlar.Where(p => p.PersonelId == parametre.PersonelId.Value);
            }

            if (parametre.DepartmanId.HasValue)
            {
                var departmanPersonelleri = await _unitOfWork.Personeller
                    .FindAsync(p => p.DepartmanId == parametre.DepartmanId.Value);
                var personelIdler = departmanPersonelleri.Select(p => p.Id).ToList();
                puantajlar = puantajlar.Where(p => personelIdler.Contains(p.PersonelId));
            }

            return new PuantajOzetRaporDTO
            {
                ToplamPersonelSayisi = puantajlar.Select(p => p.PersonelId).Distinct().Count(),
                ToplamCalismaSaati = puantajlar.Sum(p => p.ToplamCalismaSuresi) / 60.0m,
                ToplamFazlaMesai = puantajlar.Sum(p => p.FazlaMesaiSuresi) / 60.0m,
                ToplamDevamsizlik = puantajlar.Sum(p => p.DevamsizlikGunSayisi),
                ToplamIzin = puantajlar.Sum(p => p.IzinGunSayisi)
            };
        }

        public async Task<List<DepartmanPuantajOzetDTO>> GetDepartmanOzetAsync(int yil, int ay)
        {
            var departmanlar = await _unitOfWork.Departmanlar.GetAllAsync();
            var liste = new List<DepartmanPuantajOzetDTO>();

            foreach (var departman in departmanlar)
            {
                var personeller = await _unitOfWork.Personeller.FindAsync(p => p.DepartmanId == departman.Id);
                var personelIdler = personeller.Select(p => p.Id).ToList();

                var puantajlar = await _unitOfWork.Puantajlar.FindAsync(p =>
                    p.Yil == yil &&
                    p.Ay == ay &&
                    personelIdler.Contains(p.PersonelId));

                if (!puantajlar.Any())
                    continue;

                liste.Add(new DepartmanPuantajOzetDTO
                {
                    DepartmanId = departman.Id,
                    DepartmanAdi = departman.Ad,
                    PersonelSayisi = puantajlar.Count(),
                    ToplamCalismaSaati = puantajlar.Sum(p => p.ToplamCalismaSuresi) / 60.0m,
                    ToplamFazlaMesai = puantajlar.Sum(p => p.FazlaMesaiSuresi) / 60.0m,
                    ToplamDevamsizlik = puantajlar.Sum(p => p.DevamsizlikGunSayisi),
                    OrtalamaCalismaOrani = puantajlar.Any()
                        ? (decimal)puantajlar.Average(p => p.ToplamCalisilanGun) / DateTime.DaysInMonth(yil, ay) * 100
                        : 0
                });
            }

            return liste.OrderBy(d => d.DepartmanAdi).ToList();
        }

        public async Task<byte[]> ExportToExcelAsync(PuantajRaporParametreDTO parametre)
        {
            // Excel export implementasyonu - OfficeOpenXml veya ClosedXML kullanılabilir
            throw new NotImplementedException("Excel export özelliği henüz implement edilmedi");
        }

        // Yardımcı Metotlar
        public async Task<bool> PuantajVarMiAsync(int personelId, int yil, int ay)
        {
            return await _unitOfWork.Puantajlar.AnyAsync(p =>
                p.PersonelId == personelId &&
                p.Yil == yil &&
                p.Ay == ay);
        }

        public async Task<List<string>> ValidasyonKontrolAsync(int personelId, int yil, int ay)
        {
            var hatalar = new List<string>();

            // Personel kontrolü
            var personel = await _unitOfWork.Personeller.GetByIdAsync(personelId);
            if (personel == null)
            {
                hatalar.Add("Personel bulunamadı");
                return hatalar;
            }

            if (!personel.Durum)
            {
                hatalar.Add("Personel aktif değil");
            }

            // Vardiya kontrolü
            if (personel.VardiyaId == null)
            {
                hatalar.Add("Personele atanmış vardiya bulunamadı");
            }

            // Dönem kontrolü
            var baslangicTarihi = new DateTime(yil, ay, 1);
            var bitisTarihi = baslangicTarihi.AddMonths(1).AddDays(-1);

            // Giriş-Çıkış kayıtları kontrolü
            var girisler = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.PersonelId == personelId &&
                g.GirisZamani >= baslangicTarihi &&
                g.GirisZamani <= bitisTarihi);

            if (!girisler.Any())
            {
                hatalar.Add("Bu dönem için giriş-çıkış kaydı bulunamadı");
            }

            return hatalar;
        }

        // ============================================
        // PRIVATE HELPER METODLARI
        // ============================================

        private async Task PuantajHesaplaAsync(int puantajId, Personel personel, int yil, int ay)
        {
            var puantaj = await _unitOfWork.Puantajlar.GetByIdAsync(puantajId);

            // Dönem tarih aralığını belirle
            var baslangicTarihi = new DateTime(yil, ay, 1);
            var bitisTarihi = baslangicTarihi.AddMonths(1).AddDays(-1);

            // Mevcut detayları sil - DeleteRange yerine foreach
            var mevcutDetaylar = await _unitOfWork.PuantajDetaylar.FindAsync(d => d.PuantajId == puantajId);
            foreach (var detay in mevcutDetaylar)
            {
                _unitOfWork.PuantajDetaylar.Remove(detay);
            }
            await _unitOfWork.SaveChangesAsync();

            // Günlük detayları oluştur
            var gunlukDetaylar = new List<PuantajDetay>();
            for (var tarih = baslangicTarihi; tarih <= bitisTarihi; tarih = tarih.AddDays(1))
            {
                var gunTipi = TarihGunTipiBelirle(tarih);
                var girisler = await _unitOfWork.GirisCikislar.FindAsync(g =>
                    g.PersonelId == personel.Id &&
                    g.GirisZamani.Value.Date == tarih.Date);

                var izinler = await _unitOfWork.Izinler.FindAsync(i =>
                    i.PersonelId == personel.Id &&
                    i.BaslangicTarihi <= tarih &&
                    i.BitisTarihi >= tarih &&
                    i.OnayDurumu == "Onaylandı");

                var detay = new PuantajDetay
                {
                    PuantajId = puantajId,
                    Tarih = tarih,
                    GunTipi = gunTipi,
                    VardiyaId = personel.VardiyaId,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                // İzin kontrolü
                var izin = izinler.FirstOrDefault();
                if (izin != null)
                {
                    detay.IzinliMi = true;
                    detay.IzinId = izin.Id;
                    detay.IzinTuru = izin.IzinTipi;
                    detay.CalismaDurumu = "Izinli";
                }
                else if (girisler.Any())
                {
                    var ilkGiris = girisler.OrderBy(g => g.GirisZamani).First();
                    var sonCikis = girisler.OrderByDescending(g => g.GirisZamani).First();

                    detay.IlkGiris = ilkGiris.GirisZamani;
                    detay.SonCikis = sonCikis.CikisZamani;

                    // Çalışma süresi hesapla (basit)
                    if (detay.IlkGiris.HasValue && detay.SonCikis.HasValue)
                    {
                        detay.ToplamCalismaSuresi = (int)(detay.SonCikis.Value - detay.IlkGiris.Value).TotalMinutes;
                    }

                    // Vardiya bilgilerine göre geç kalma/erken çıkış hesapla
                    if (personel.VardiyaId.HasValue)
                    {
                        var vardiya = await _unitOfWork.Vardiyalar.GetByIdAsync(personel.VardiyaId.Value);
                        if (vardiya != null)
                        {
                            detay.VardiyaBaslangic = vardiya.BaslangicSaati;
                            detay.VardiyaBitis = vardiya.BitisSaati;
                            detay.PlanlananCalismaSuresi = (int)(vardiya.BitisSaati - vardiya.BaslangicSaati).TotalMinutes;

                            // Geç kalma kontrolü
                            var vardiyaBaslangicDateTime = tarih.Date + vardiya.BaslangicSaati;
                            if (detay.IlkGiris > vardiyaBaslangicDateTime)
                            {
                                detay.GecKalmaSuresi = (int)(detay.IlkGiris.Value - vardiyaBaslangicDateTime).TotalMinutes;
                            }

                            // Erken çıkış kontrolü
                            var vardiyaBitisDateTime = tarih.Date + vardiya.BitisSaati;
                            if (detay.SonCikis < vardiyaBitisDateTime)
                            {
                                detay.ErkenCikisSuresi = (int)(vardiyaBitisDateTime - detay.SonCikis.Value).TotalMinutes;
                            }

                            // Fazla mesai kontrolü
                            if (detay.SonCikis > vardiyaBitisDateTime)
                            {
                                detay.FazlaMesaiSuresi = (int)(detay.SonCikis.Value - vardiyaBitisDateTime).TotalMinutes;
                            }
                        }
                    }

                    detay.CalismaDurumu = "Normal";
                }
                else
                {
                    if (gunTipi == 2)
                        detay.CalismaDurumu = "HaftaTatili";
                    else if (gunTipi == 3)
                        detay.CalismaDurumu = "ResmiTatil";
                    else
                        detay.CalismaDurumu = "Devamsiz";
                }

                gunlukDetaylar.Add(detay);
            }

            // Detayları kaydet
            await _unitOfWork.PuantajDetaylar.AddRangeAsync(gunlukDetaylar);
            await _unitOfWork.SaveChangesAsync();

            // Puantaj özetini güncelle
            puantaj.ToplamCalismaSuresi = gunlukDetaylar.Sum(d => d.ToplamCalismaSuresi ?? 0);
            puantaj.ToplamCalisilanGun = gunlukDetaylar.Count(d => d.ToplamCalismaSuresi.HasValue && d.ToplamCalismaSuresi > 0);
            puantaj.FazlaMesaiSuresi = gunlukDetaylar.Sum(d => d.FazlaMesaiSuresi ?? 0);
            puantaj.ToplamGecKalmaSuresi = gunlukDetaylar.Sum(d => d.GecKalmaSuresi ?? 0);
            puantaj.ToplamErkenCikisSuresi = gunlukDetaylar.Sum(d => d.ErkenCikisSuresi ?? 0);
            puantaj.GecKalmaGunSayisi = gunlukDetaylar.Count(d => d.GecKalmaSuresi.HasValue && d.GecKalmaSuresi > 0);
            puantaj.ErkenCikisGunSayisi = gunlukDetaylar.Count(d => d.ErkenCikisSuresi.HasValue && d.ErkenCikisSuresi > 0);
            puantaj.DevamsizlikGunSayisi = gunlukDetaylar.Count(d => d.CalismaDurumu == "Devamsiz");
            puantaj.IzinGunSayisi = gunlukDetaylar.Count(d => d.IzinliMi);
            puantaj.HaftaTatiliGunSayisi = gunlukDetaylar.Count(d => d.GunTipi == 2);
            puantaj.ResmiTatilGunSayisi = gunlukDetaylar.Count(d => d.GunTipi == 3);

            puantaj.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.Puantajlar.Update(puantaj);
            await _unitOfWork.SaveChangesAsync();
        }

        private int TarihGunTipiBelirle(DateTime tarih)
        {
            // 1: Hafta İçi, 2: Hafta Tatili, 3: Resmi Tatil
            if (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                return 2;

            // Resmi tatil kontrolü yapılabilir
            return 1;
        }

        public async Task<IEnumerable<PuantajListDTO>> GetAllAsync(int sirketId, int yil, int ay)
        {
            var puantajlar = await _unitOfWork.Puantajlar.FindAsync(p =>
                p.SirketId == sirketId &&
                p.Yil == yil &&
                p.Ay == ay);

            var liste = new List<PuantajListDTO>();

            foreach (var p in puantajlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(p.PersonelId);
                if (personel == null) continue;

                var departman = personel.DepartmanId != null
                    ? await _unitOfWork.Departmanlar.GetByIdAsync(personel.DepartmanId.Value)
                    : null;

                liste.Add(new PuantajListDTO
                {
                    Id = p.Id,
                    SirketId = p.SirketId,
                    PersonelId = p.PersonelId,
                    PersonelAdSoyad = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    DepartmanAdi = departman?.Ad ?? "-",
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
                });
            }

            return liste.OrderBy(p => p.PersonelAdSoyad);
        }

        /// <summary>
        /// Personel ve dönem bazlı puantaj detayı getirir (3 parametreli overload)
        /// </summary>
        public async Task<PuantajDetailDTO> GetByPersonelAsync(int personelId, int yil, int ay)
        {
            return await GetByPersonelVeDonemAsync(personelId, yil, ay);
        }

        /// <summary>
        /// Tek puantaj oluşturur
        /// </summary>
        public async Task<int> OlusturAsync(PuantajCreateDTO dto)
        {
            var hesaplaDto = new PuantajHesaplaDTO
            {
                PersonelId = dto.PersonelId,
                Yil = dto.Yil,
                Ay = dto.Ay,
                YenidenHesapla = dto.YenidenHesapla
            };

            return await HesaplaPuantajAsync(hesaplaDto);
        }

        /// <summary>
        /// Toplu puantaj oluşturur
        /// </summary>
        public async Task<List<int>> TopluOlusturAsync(PuantajTopluOlusturDTO dto)
        {
            var topluHesaplaDto = new TopluPuantajHesaplaDTO
            {
                SirketId = dto.SirketId,
                Yil = dto.Yil,
                Ay = dto.Ay,
                TumPersonel = dto.TumPersonel,
                DepartmanId = dto.DepartmanId,
                PersonelIdler = dto.PersonelIdler
            };

            return await TopluPuantajHesaplaAsync(topluHesaplaDto);
        }

        /// <summary>
        /// Puantaj istatistiklerini getirir
        /// </summary>
        public async Task<object> GetIstatistikAsync(int sirketId, int yil, int ay)
        {
            var puantajlar = await _unitOfWork.Puantajlar.FindAsync(p =>
                p.SirketId == sirketId &&
                p.Yil == yil &&
                p.Ay == ay);

            var puantajList = puantajlar.ToList();

            return new
            {
                ToplamPersonel = puantajList.Count,
                OnaylananSayisi = puantajList.Count(p => p.Onaylandi),
                OnayBekleyenSayisi = puantajList.Count(p => !p.Onaylandi),
                ToplamCalismaSaati = puantajList.Sum(p => p.ToplamCalismaSuresi) / 60.0m,
                ToplamFazlaMesai = puantajList.Sum(p => p.FazlaMesaiSuresi) / 60.0m,
                ToplamDevamsizlik = puantajList.Sum(p => p.DevamsizlikGunSayisi),
                ToplamIzin = puantajList.Sum(p => p.IzinGunSayisi),
                OrtalamaCalismaGunu = puantajList.Count > 0 ? puantajList.Average(p => p.ToplamCalisilanGun) : 0
            };
        }

        /// <summary>
        /// Geç kalanlar raporunu getirir
        /// </summary>
        public async Task<List<GecKalanlarRaporDTO>> GetGecKalanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.Personel.SirketId == sirketId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.GecKalmaSuresi > 0);

            var result = new List<GecKalanlarRaporDTO>();

            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                if (personel == null) continue;

                result.Add(new GecKalanlarRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelId = personel.Id,
                    PersonelAdSoyad = personel.AdSoyad,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    DepartmanAdi = personel.Departman?.Ad ?? "-",
                    Departman = personel.Departman?.Ad ?? "-",
                    VardiyaBaslangic = personel.Vardiya?.BaslangicSaati ?? TimeSpan.Zero,
                    BeklenenGiris = personel.Vardiya?.BaslangicSaati.ToString(@"hh\:mm") ?? "",
                    GirisSaati = kayit.GirisZamani,
                    GercekGiris = kayit.GirisZamani?.ToString("HH:mm") ?? "",
                    GecKalmaSuresi = kayit.GecKalmaSuresi ?? 0
                });
            }

            return result.OrderByDescending(r => r.GecKalmaSuresi).ToList();
        }

        /// <summary>
        /// Erken çıkanlar raporunu getirir
        /// </summary>
        public async Task<List<ErkenCikanlarRaporDTO>> GetErkenCikanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.Personel.SirketId == sirketId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.ErkenCikisSuresi > 0);

            var result = new List<ErkenCikanlarRaporDTO>();

            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                if (personel == null) continue;

                result.Add(new ErkenCikanlarRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelId = personel.Id,
                    PersonelAdSoyad = personel.AdSoyad,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    DepartmanAdi = personel.Departman?.Ad ?? "-",
                    Departman = personel.Departman?.Ad ?? "-",
                    VardiyaBitis = personel.Vardiya?.BitisSaati ?? TimeSpan.Zero,
                    BeklenenCikis = personel.Vardiya?.BitisSaati.ToString(@"hh\:mm") ?? "",
                    CikisSaati = kayit.CikisZamani,
                    GercekCikis = kayit.CikisZamani?.ToString("HH:mm") ?? "",
                    ErkenCikisSuresi = kayit.ErkenCikisSuresi ?? 0
                });
            }

            return result.OrderByDescending(r => r.ErkenCikisSuresi).ToList();
        }

        /// <summary>
        /// Fazla mesai raporunu getirir
        /// </summary>
        public async Task<List<FazlaMesaiRaporDTO>> GetFazlaMesaiRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var kayitlar = await _unitOfWork.GirisCikislar.FindAsync(g =>
                g.Personel.SirketId == sirketId &&
                g.GirisZamani >= baslangic &&
                g.GirisZamani <= bitis &&
                g.FazlaMesaiSuresi > 0);

            var result = new List<FazlaMesaiRaporDTO>();

            foreach (var kayit in kayitlar)
            {
                var personel = await _unitOfWork.Personeller.GetByIdAsync(kayit.PersonelId);
                if (personel == null) continue;

                var fazlaMesaiUcreti = CalculateOvertimePay(personel.Maas ?? 0m, kayit.FazlaMesaiSuresi ?? 0);

                result.Add(new FazlaMesaiRaporDTO
                {
                    Tarih = kayit.GirisZamani?.Date ?? DateTime.UtcNow,
                    PersonelId = personel.Id,
                    PersonelAdSoyad = personel.AdSoyad,
                    PersonelAdi = personel.AdSoyad,
                    SicilNo = personel.SicilNo,
                    DepartmanAdi = personel.Departman?.Ad ?? "-",
                    Departman = personel.Departman?.Ad ?? "-",
                    FazlaMesaiSuresi = kayit.FazlaMesaiSuresi ?? 0,
                    FazlaMesaiUcreti = fazlaMesaiUcreti
                });
            }

            return result.OrderByDescending(r => r.FazlaMesaiSuresi).ToList();
        }

        /// <summary>
        /// Devamsızlık raporunu getirir
        /// </summary>
        public async Task<List<DevamsizlarRaporDTO>> GetDevamsizlikRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis)
        {
            var aktifPersoneller = await _unitOfWork.Personeller.FindAsync(p =>
                p.SirketId == sirketId &&
                p.Durum);

            var result = new List<DevamsizlarRaporDTO>();

            foreach (var personel in aktifPersoneller)
            {
                for (var tarih = baslangic; tarih <= bitis; tarih = tarih.AddDays(1))
                {
                    // Hafta sonu kontrolü
                    if (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    // Tatil kontrolü
                    var tatil = await _unitOfWork.Tatiller.FirstOrDefaultAsync(t => t.Tarih.Date == tarih.Date);
                    if (tatil != null)
                        continue;

                    // İzin kontrolü
                    var izin = await _unitOfWork.Izinler.FirstOrDefaultAsync(i =>
                        i.PersonelId == personel.Id &&
                        i.BaslangicTarihi <= tarih &&
                        i.BitisTarihi >= tarih &&
                        i.OnayDurumu == "Onaylandi");

                    if (izin != null)
                        continue;

                    // Giriş-çıkış kaydı kontrolü
                    var giris = await _unitOfWork.GirisCikislar.FirstOrDefaultAsync(g =>
                        g.PersonelId == personel.Id &&
                        g.GirisZamani.HasValue &&
                        g.GirisZamani.Value.Date == tarih.Date);

                    if (giris == null)
                    {
                        result.Add(new DevamsizlarRaporDTO
                        {
                            Tarih = tarih,
                            PersonelId = personel.Id,
                            PersonelAdi = personel.AdSoyad,
                            SicilNo = personel.SicilNo,
                            Departman = personel.Departman?.Ad ?? "-",
                            DevamsizlikSebep = "Giriş kaydı yok"
                        });
                    }
                }
            }

            return result.OrderBy(r => r.Tarih).ThenBy(r => r.PersonelAdi).ToList();
        }

        /// <summary>
        /// Fazla mesai ücretini hesaplar
        /// </summary>
        private decimal CalculateOvertimePay(decimal monthlyGrossSalary, int overtimeMinutes)
        {
            if (monthlyGrossSalary <= 0 || overtimeMinutes <= 0)
                return 0;

            // Aylık çalışma saati: 225 saat (standart)
            decimal hourlyRate = monthlyGrossSalary / 225m;

            // Fazla mesai çarpanı: 1.5
            decimal overtimeRate = hourlyRate * 1.5m;

            // Dakikayı saate çevir ve ücret hesapla
            decimal overtimeHours = overtimeMinutes / 60m;

            return overtimeHours * overtimeRate;
        }
    }
}
