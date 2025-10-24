// PDKS.Business/Services/OnayAkisiService.cs - DÜZELTİLMİŞ

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
    public class OnayAkisiService : IOnayAkisiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBildirimService _bildirimService;

        public OnayAkisiService(
            IUnitOfWork unitOfWork,
            IBildirimService bildirimService)
        {
            _unitOfWork = unitOfWork;
            _bildirimService = bildirimService;
        }

        public async Task<IEnumerable<OnayAkisi>> GetAllOnayAkislariAsync(int sirketId)
        {
            return await _unitOfWork.OnayAkislari
                .FindAsync(x => x.SirketId == sirketId);
        }

        public async Task<OnayAkisi> GetOnayAkisiByIdAsync(int id)
        {
            return await _unitOfWork.OnayAkislari
                .GetByIdAsync(id);
        }

        public async Task<OnayAkisi> CreateOnayAkisiAsync(OnayAkisiDTO dto)
        {
            var onayAkisi = new OnayAkisi
            {
                SirketId = dto.SirketId,
                AkisAdi = dto.AkisAdi,
                ModulTipi = dto.ModulTipi,
                Aciklama = dto.Aciklama,
                Aktif = dto.Aktif,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.OnayAkislari.AddAsync(onayAkisi);
            await _unitOfWork.SaveChangesAsync();

            // Adımları ekle
            foreach (var adimDTO in dto.OnayAdimlari.OrderBy(x => x.Sira))
            {
                var adim = new OnayAdimi
                {
                    OnayAkisiId = onayAkisi.Id,
                    Sira = adimDTO.Sira,
                    AdimAdi = adimDTO.AdimAdi,
                    OnaylayanTipi = adimDTO.OnaylayanTipi,
                    OnaylayanRolId = adimDTO.OnaylayanRolId,
                    OnaylayanKullaniciId = adimDTO.OnaylayanKullaniciId,
                    OnaylayanDepartmanId = adimDTO.OnaylayanDepartmanId,
                    Zorunlu = adimDTO.Zorunlu,
                    TimeoutGun = adimDTO.TimeoutGun,
                    EscalateKullaniciId = adimDTO.EscalateKullaniciId,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.OnayAdimlari.AddAsync(adim);
            }

            await _unitOfWork.SaveChangesAsync();

            return onayAkisi;
        }

        public async Task<OnayAkisi> UpdateOnayAkisiAsync(int id, OnayAkisiDTO dto)
        {
            var onayAkisi = await _unitOfWork.OnayAkislari
                .GetByIdAsync(id);

            if (onayAkisi == null)
                throw new Exception("Onay akışı bulunamadı");

            // Güncelle
            onayAkisi.AkisAdi = dto.AkisAdi;
            onayAkisi.ModulTipi = dto.ModulTipi;
            onayAkisi.Aciklama = dto.Aciklama;
            onayAkisi.Aktif = dto.Aktif;
            onayAkisi.GuncellemeTarihi = DateTime.UtcNow;

            // Eski adımları sil
            var eskiAdimlar = await _unitOfWork.OnayAdimlari
                .FindAsync(x => x.OnayAkisiId == id);

            foreach (var eskiAdim in eskiAdimlar)
            {
                _unitOfWork.OnayAdimlari.Delete(eskiAdim);
            }

            await _unitOfWork.SaveChangesAsync();

            // Yeni adımları ekle
            foreach (var adimDTO in dto.OnayAdimlari.OrderBy(x => x.Sira))
            {
                var adim = new OnayAdimi
                {
                    OnayAkisiId = onayAkisi.Id,
                    Sira = adimDTO.Sira,
                    AdimAdi = adimDTO.AdimAdi,
                    OnaylayanTipi = adimDTO.OnaylayanTipi,
                    OnaylayanRolId = adimDTO.OnaylayanRolId,
                    OnaylayanKullaniciId = adimDTO.OnaylayanKullaniciId,
                    OnaylayanDepartmanId = adimDTO.OnaylayanDepartmanId,
                    Zorunlu = adimDTO.Zorunlu,
                    TimeoutGun = adimDTO.TimeoutGun,
                    EscalateKullaniciId = adimDTO.EscalateKullaniciId,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.OnayAdimlari.AddAsync(adim);
            }

            _unitOfWork.OnayAkislari.Update(onayAkisi);
            await _unitOfWork.SaveChangesAsync();

            return onayAkisi;
        }

        public async Task<bool> DeleteOnayAkisiAsync(int id)
        {
            var onayAkisi = await _unitOfWork.OnayAkislari
                .GetByIdAsync(id);

            if (onayAkisi == null)
                throw new Exception("Onay akışı bulunamadı");

            // Kullanımda olan akış silinemez
            var kullanimdakiKayitlar = await _unitOfWork.OnayKayitlari
                .FindAsync(x => x.OnayAkisiId == id);

            if (kullanimdakiKayitlar.Any())
                throw new Exception("Bu onay akışı kullanımda olduğu için silinemez");

            _unitOfWork.OnayAkislari.Delete(onayAkisi);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<OnayAkisi> GetOnayAkisiByModulAsync(string modulTipi, int sirketId)
        {
            var akislar = await _unitOfWork.OnayAkislari
                .FindAsync(x => x.ModulTipi == modulTipi && x.SirketId == sirketId && x.Aktif);

            return akislar.FirstOrDefault();
        }

        public async Task<OnayKaydi> BaslatOnayAsync(OnayBaslatDTO dto)
        {
            // Kullanıcı bilgisini al
            var talepEden = await _unitOfWork.Kullanicilar.GetByIdAsync(dto.TalepEdenKullaniciId);

            if (talepEden == null || talepEden.Personel == null)
                throw new Exception("Kullanıcı bulunamadı");

            // İlgili modül için onay akışını bul
            var onayAkisi = await GetOnayAkisiByModulAsync(dto.ModulTipi, talepEden.Personel.SirketId);

            if (onayAkisi == null)
                throw new Exception($"{dto.ModulTipi} için onay akışı tanımlanmamış");

            // OnayKaydi oluştur
            var onayKaydi = new OnayKaydi
            {
                OnayAkisiId = onayAkisi.Id,
                ModulTipi = dto.ModulTipi,
                ReferansId = dto.ReferansId,
                TalepEdenKullaniciId = dto.TalepEdenKullaniciId,
                MevcutAdimSira = 1,
                GenelDurum = "Beklemede",
                TalepTarihi = DateTime.UtcNow,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.OnayKayitlari.AddAsync(onayKaydi);
            await _unitOfWork.SaveChangesAsync();

            // Adımları çek
            var adimlar = await _unitOfWork.OnayAdimlari
                .FindAsync(x => x.OnayAkisiId == onayAkisi.Id);

            // Her adım için OnayDetay oluştur
            foreach (var adim in adimlar.OrderBy(x => x.Sira))
            {
                var detay = new OnayDetay
                {
                    OnayKaydiId = onayKaydi.Id,
                    OnayAdimiId = adim.Id,
                    AdimSira = adim.Sira,
                    Durum = adim.Sira == 1 ? "Beklemede" : "BeklemedeKaldi",
                    OlusturmaTarihi = DateTime.UtcNow
                };

                await _unitOfWork.OnayDetaylari.AddAsync(detay);
            }

            await _unitOfWork.SaveChangesAsync();

            // İlk onaylayıcıya bildirim gönder
            var ilkAdim = adimlar.OrderBy(x => x.Sira).FirstOrDefault();
            if (ilkAdim != null)
            {
                await BildirimGonderAsync(onayKaydi, ilkAdim);
            }

            return onayKaydi;
        }

        public async Task<bool> OnaylaAsync(int onayKaydiId, int kullaniciId, string aciklama)
        {
            var onayKaydi = await _unitOfWork.OnayKayitlari
                .GetByIdAsync(onayKaydiId);

            if (onayKaydi == null)
                throw new Exception("Onay kaydı bulunamadı");

            // Detayları ve adımları çek
            var detaylar = await _unitOfWork.OnayDetaylari
                .FindAsync(x => x.OnayKaydiId == onayKaydiId);

            var adimlar = await _unitOfWork.OnayAdimlari
                .FindAsync(x => x.OnayAkisiId == onayKaydi.OnayAkisiId);

            // Mevcut adımı bul
            var mevcutDetay = detaylar.FirstOrDefault(x => x.AdimSira == onayKaydi.MevcutAdimSira && x.Durum == "Beklemede");

            if (mevcutDetay == null)
                throw new Exception("Onaylanacak adım bulunamadı");

            // Yetki kontrolü
            var adim = adimlar.First(x => x.Id == mevcutDetay.OnayAdimiId);
            if (!await YetkiKontroluAsync(adim, kullaniciId))
                throw new UnauthorizedAccessException("Bu adımı onaylama yetkiniz yok");

            // Onayla
            mevcutDetay.OnaylayanKullaniciId = kullaniciId;
            mevcutDetay.Durum = "Onaylandi";
            mevcutDetay.OnayTarihi = DateTime.UtcNow;
            mevcutDetay.Aciklama = aciklama;
            mevcutDetay.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.OnayDetaylari.Update(mevcutDetay);

            // Son adım mı?
            var sonAdim = adimlar.Max(x => x.Sira);
            if (onayKaydi.MevcutAdimSira == sonAdim)
            {
                // Tamamlandı
                onayKaydi.GenelDurum = "Onaylandi";
                onayKaydi.TamamlanmaTarihi = DateTime.UtcNow;

                // Talep edene bildirim
                await _bildirimService.BildirimGonderAsync(
                    onayKaydi.TalepEdenKullaniciId,
                    "Talep Onaylandı",
                    $"{onayKaydi.ModulTipi} talebiniz onaylandı",
                    "Success"
                );
            }
            else
            {
                // Sonraki adıma geç
                onayKaydi.MevcutAdimSira++;

                var sonrakiDetay = detaylar.First(x => x.AdimSira == onayKaydi.MevcutAdimSira);
                sonrakiDetay.Durum = "Beklemede";
                _unitOfWork.OnayDetaylari.Update(sonrakiDetay);

                var sonrakiAdim = adimlar.First(x => x.Sira == onayKaydi.MevcutAdimSira);
                await BildirimGonderAsync(onayKaydi, sonrakiAdim);
            }

            onayKaydi.GuncellemeTarihi = DateTime.UtcNow;
            _unitOfWork.OnayKayitlari.Update(onayKaydi);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReddetAsync(int onayKaydiId, int kullaniciId, string aciklama)
        {
            var onayKaydi = await _unitOfWork.OnayKayitlari
                .GetByIdAsync(onayKaydiId);

            if (onayKaydi == null)
                throw new Exception("Onay kaydı bulunamadı");

            var detaylar = await _unitOfWork.OnayDetaylari
                .FindAsync(x => x.OnayKaydiId == onayKaydiId);

            var mevcutDetay = detaylar.FirstOrDefault(x => x.AdimSira == onayKaydi.MevcutAdimSira && x.Durum == "Beklemede");

            if (mevcutDetay == null)
                throw new Exception("Reddedilecek adım bulunamadı");

            // Reddet
            mevcutDetay.OnaylayanKullaniciId = kullaniciId;
            mevcutDetay.Durum = "Reddedildi";
            mevcutDetay.OnayTarihi = DateTime.UtcNow;
            mevcutDetay.Aciklama = aciklama;
            mevcutDetay.GuncellemeTarihi = DateTime.UtcNow;

            onayKaydi.GenelDurum = "Reddedildi";
            onayKaydi.TamamlanmaTarihi = DateTime.UtcNow;
            onayKaydi.GuncellemeTarihi = DateTime.UtcNow;

            _unitOfWork.OnayDetaylari.Update(mevcutDetay);
            _unitOfWork.OnayKayitlari.Update(onayKaydi);
            await _unitOfWork.SaveChangesAsync();

            // Talep edene bildirim
            await _bildirimService.BildirimGonderAsync(
                onayKaydi.TalepEdenKullaniciId,
                "Talep Reddedildi",
                $"{onayKaydi.ModulTipi} talebiniz reddedildi. Sebep: {aciklama}",
                "Error"
            );

            return true;
        }

        // PDKS.Business/Services/OnayAkisiService.cs - GetBekleyenOnaylarAsync

        public async Task<IEnumerable<BekleyenOnayDTO>> GetBekleyenOnaylarAsync(int kullaniciId)
        {
            // Kullanıcıyı bul
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);

            if (kullanici == null)
            {
                throw new Exception($"Kullanıcı bulunamadı. Aranan ID: {kullaniciId}");
            }

            // Personel bilgisini yükle - PersonelId nullable mı kontrol et
            if (kullanici.PersonelId != null && kullanici.PersonelId != 0)
            {
                var personelId = kullanici.PersonelId is int ? (int)kullanici.PersonelId : kullanici.PersonelId;
                kullanici.Personel = await _unitOfWork.Personeller.GetByIdAsync((int)personelId);

                if (kullanici.Personel != null)
                {
                    var departmanId = kullanici.Personel.DepartmanId;
                    if (departmanId != null && departmanId != 0)
                    {
                        kullanici.Personel.Departman = await _unitOfWork.Departmanlar.GetByIdAsync((int)departmanId);
                    }
                }
            }

            // Rol bilgisini yükle
            if (kullanici.RolId != null && kullanici.RolId != 0)
            {
                var rolId = kullanici.RolId is int ? (int)kullanici.RolId : kullanici.RolId;
                kullanici.Rol = await _unitOfWork.Roller.GetByIdAsync((int)rolId);
            }

            // Bekleyen onayları çek
            var bekleyenOnaylar = await _unitOfWork.OnayKayitlari
                .FindAsync(x => x.GenelDurum == "Beklemede");

            var sonuc = new List<BekleyenOnayDTO>();

            foreach (var kayit in bekleyenOnaylar)
            {
                var adimlar = await _unitOfWork.OnayAdimlari
                    .FindAsync(x => x.OnayAkisiId == kayit.OnayAkisiId);

                var mevcutAdim = adimlar.FirstOrDefault(x => x.Sira == kayit.MevcutAdimSira);

                if (mevcutAdim != null && await YetkiKontroluAsync(mevcutAdim, kullaniciId))
                {
                    var talepEden = await _unitOfWork.Kullanicilar.GetByIdAsync(kayit.TalepEdenKullaniciId);

                    // Talep edenin personel bilgisini yükle
                    if (talepEden != null && talepEden.PersonelId != null && talepEden.PersonelId != 0)
                    {
                        var personelId = talepEden.PersonelId is int ? (int)talepEden.PersonelId : talepEden.PersonelId;
                        talepEden.Personel = await _unitOfWork.Personeller.GetByIdAsync((int)personelId);

                        if (talepEden.Personel != null)
                        {
                            var departmanId = talepEden.Personel.DepartmanId;
                            if (departmanId != null && departmanId != 0)
                            {
                                talepEden.Personel.Departman = await _unitOfWork.Departmanlar.GetByIdAsync((int)departmanId);
                            }
                        }
                    }

                    var beklemeSuresi = (DateTime.UtcNow - kayit.TalepTarihi).Days;
                    var oncelik = mevcutAdim.TimeoutGun.HasValue &&
                                  beklemeSuresi >= (mevcutAdim.TimeoutGun.Value - 2)
                        ? "Acil"
                        : "Normal";

                    sonuc.Add(new BekleyenOnayDTO
                    {
                        OnayKaydiId = kayit.Id,
                        ModulTipi = kayit.ModulTipi,
                        ReferansId = kayit.ReferansId,
                        TalepEdenKisi = talepEden?.Personel?.AdSoyad ?? "Bilinmiyor",
                        TalepEdenDepartman = talepEden?.Personel?.Departman?.Ad ?? "-",
                        TalepTarihi = kayit.TalepTarihi,
                        AdimAdi = mevcutAdim.AdimAdi,
                        BeklemeSuresi = beklemeSuresi,
                        OncelikDurumu = oncelik
                    });
                }
            }

            return sonuc.OrderByDescending(x => x.OncelikDurumu).ThenByDescending(x => x.BeklemeSuresi);
        }

        public async Task<OnayDurumuDTO> GetOnayDurumuAsync(string modulTipi, int referansId)
        {
            var kayitlar = await _unitOfWork.OnayKayitlari
                .FindAsync(x => x.ModulTipi == modulTipi && x.ReferansId == referansId);

            var onayKaydi = kayitlar.FirstOrDefault();

            if (onayKaydi == null)
                return null;

            var adimlar = await _unitOfWork.OnayAdimlari
                .FindAsync(x => x.OnayAkisiId == onayKaydi.OnayAkisiId);

            var detaylar = await _unitOfWork.OnayDetaylari
                .FindAsync(x => x.OnayKaydiId == onayKaydi.Id);

            var mevcutAdim = adimlar.FirstOrDefault(x => x.Sira == onayKaydi.MevcutAdimSira);

            var detayListesi = new List<OnayDetayDTO>();

            foreach (var detay in detaylar.OrderBy(x => x.AdimSira))
            {
                var adim = adimlar.First(x => x.Id == detay.OnayAdimiId);

                string onaylayanKisi = "-";
                if (detay.OnaylayanKullaniciId.HasValue)
                {
                    var onaylayan = await _unitOfWork.Kullanicilar.GetByIdAsync(detay.OnaylayanKullaniciId.Value);
                    onaylayanKisi = onaylayan?.Personel?.AdSoyad ?? "-";
                }

                detayListesi.Add(new OnayDetayDTO
                {
                    AdimSira = detay.AdimSira,
                    AdimAdi = adim.AdimAdi,
                    OnaylayanKisi = onaylayanKisi,
                    Durum = detay.Durum,
                    OnayTarihi = detay.OnayTarihi,
                    Aciklama = detay.Aciklama
                });
            }

            var talepEden = await _unitOfWork.Kullanicilar.GetByIdAsync(onayKaydi.TalepEdenKullaniciId);

            return new OnayDurumuDTO
            {
                OnayKaydiId = onayKaydi.Id,
                ModulTipi = onayKaydi.ModulTipi,
                ReferansId = onayKaydi.ReferansId,
                TalepEdenKisi = talepEden?.Personel?.AdSoyad ?? "Bilinmiyor",
                TalepTarihi = onayKaydi.TalepTarihi,
                GenelDurum = onayKaydi.GenelDurum,
                MevcutAdimSira = onayKaydi.MevcutAdimSira,
                MevcutAdimAdi = mevcutAdim?.AdimAdi ?? "-",
                OnayDetaylari = detayListesi
            };
        }

        public async Task<IEnumerable<OnayDurumuDTO>> GetKullaniciTaleplerAsync(int kullaniciId)
        {
            var kayitlar = await _unitOfWork.OnayKayitlari
                .FindAsync(x => x.TalepEdenKullaniciId == kullaniciId);

            var sonuc = new List<OnayDurumuDTO>();

            foreach (var kayit in kayitlar.OrderByDescending(x => x.TalepTarihi))
            {
                var dto = await GetOnayDurumuAsync(kayit.ModulTipi, kayit.ReferansId);
                if (dto != null)
                    sonuc.Add(dto);
            }

            return sonuc;
        }

        private async Task<bool> YetkiKontroluAsync(OnayAdimi adim, int kullaniciId)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);

            if (kullanici == null) return false;

            // Direkt kullanıcı kontrolü
            if (adim.OnaylayanKullaniciId.HasValue && adim.OnaylayanKullaniciId == kullaniciId)
                return true;

            // Rol kontrolü
            if (adim.OnaylayanRolId.HasValue && adim.OnaylayanRolId == kullanici.RolId)
                return true;

            // Departman müdürü kontrolü
            if (adim.OnaylayanTipi == "DepartmanMuduru")
            {
                // TODO: Departman müdürü mantığı
            }

            return false;
        }

        private async Task BildirimGonderAsync(OnayKaydi onayKaydi, OnayAdimi adim)
        {
            int? onaylayanId = null;

            if (adim.OnaylayanKullaniciId.HasValue)
            {
                onaylayanId = adim.OnaylayanKullaniciId;
            }
            else if (adim.OnaylayanRolId.HasValue)
            {
                var kullanicilar = await _unitOfWork.Kullanicilar
                    .FindAsync(x => x.RolId == adim.OnaylayanRolId && x.Aktif);
                onaylayanId = kullanicilar.FirstOrDefault()?.Id;
            }

            if (onaylayanId.HasValue)
            {
                await _bildirimService.BildirimGonderAsync(
                    onaylayanId.Value,
                    "Yeni Onay Talebi",
                    $"{onayKaydi.ModulTipi} için onay bekliyor: {adim.AdimAdi}",
                    "Info",
                    referansTip: onayKaydi.ModulTipi,
                    referansId: onayKaydi.ReferansId
                );
            }
        }
    }
}