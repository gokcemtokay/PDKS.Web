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

        public OnayAkisiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OnayAkisi>> GetBekleyenOnaylarAsync(int personelId)
        {
            return await _unitOfWork.GetRepository<OnayAkisi>()
                .FindAsync(o => o.OnaylayiciPersonelId == personelId && o.OnayDurumu == "Beklemede");
        }

        public async Task<bool> OnaylaAsync(int onayAkisiId, bool onaylandi, string aciklama, int onaylayiciId)
        {
            var onayAkisiRepo = _unitOfWork.GetRepository<OnayAkisi>();
            var onayAkisi = await onayAkisiRepo.GetByIdAsync(onayAkisiId);

            if (onayAkisi == null || onayAkisi.OnaylayiciPersonelId != onaylayiciId)
                return false;

            onayAkisi.OnayDurumu = onaylandi ? "Onaylandi" : "Reddedildi";
            onayAkisi.OnayTarihi = DateTime.UtcNow;
            onayAkisi.Aciklama = aciklama;

            onayAkisiRepo.Update(onayAkisi);
            await _unitOfWork.SaveChangesAsync();

            // Tüm onaylar tamamlandı mı kontrol et
            await UpdateTalepDurumuAsync(onayAkisi.OnayTipi, onayAkisi.ReferansId);

            return true;
        }

        public async Task<IEnumerable<OnayAkisi>> GetOnayGecmisiAsync(string onayTipi, int referansId)
        {
            return await _unitOfWork.GetRepository<OnayAkisi>()
                .FindAsync(o => o.OnayTipi == onayTipi && o.ReferansId == referansId);
        }

        private async Task UpdateTalepDurumuAsync(string onayTipi, int referansId)
        {
            var tumOnaylar = await _unitOfWork.GetRepository<OnayAkisi>()
                .FindAsync(o => o.OnayTipi == onayTipi && o.ReferansId == referansId);

            bool hepsiOnaylandi = tumOnaylar.All(o => o.OnayDurumu == "Onaylandi");
            bool redEdildi = tumOnaylar.Any(o => o.OnayDurumu == "Reddedildi");

            string durum = redEdildi ? "Reddedildi" : (hepsiOnaylandi ? "Onaylandi" : "Beklemede");

            switch (onayTipi)
            {
                case "Izin":
                    var izinRepo = _unitOfWork.Izinler;
                    var izin = await izinRepo.GetByIdAsync(referansId);
                    if (izin != null)
                    {
                        izin.OnayDurumu = durum;
                        izinRepo.Update(izin);
                    }
                    break;

                case "Avans":
                    var avansRepo = _unitOfWork.GetRepository<AvansTalebi>();
                    var avans = await avansRepo.GetByIdAsync(referansId);
                    if (avans != null)
                    {
                        avans.OnayDurumu = durum;
                        avansRepo.Update(avans);
                    }
                    break;

                case "Masraf":
                    var masrafRepo = _unitOfWork.GetRepository<MasrafTalebi>();
                    var masraf = await masrafRepo.GetByIdAsync(referansId);
                    if (masraf != null)
                    {
                        masraf.OnayDurumu = durum;
                        masrafRepo.Update(masraf);
                    }
                    break;

                case "Arac":
                    var aracRepo = _unitOfWork.GetRepository<AracTalebi>();
                    var arac = await aracRepo.GetByIdAsync(referansId);
                    if (arac != null)
                    {
                        arac.OnayDurumu = durum;
                        aracRepo.Update(arac);
                    }
                    break;

                case "Seyahat":
                    var seyahatRepo = _unitOfWork.GetRepository<SeyahatTalebi>();
                    var seyahat = await seyahatRepo.GetByIdAsync(referansId);
                    if (seyahat != null)
                    {
                        seyahat.OnayDurumu = durum;
                        seyahatRepo.Update(seyahat);
                    }
                    break;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}