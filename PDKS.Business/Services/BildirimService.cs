// PDKS.Business/Services/BildirimService.cs - DÜZELTİLMİŞ

using Microsoft.AspNetCore.SignalR;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public class BildirimService : IBildirimService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<Hub> _hubContext; // ⬅️ Generic Hub kullan
        private readonly IPushNotificationService _pushNotificationService;

        public BildirimService(
            IUnitOfWork unitOfWork,
            IHubContext<Hub> hubContext, // ⬅️ Değişti
            IPushNotificationService pushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _pushNotificationService = pushNotificationService;
        }

        // ... rest of the methods stay the same

        public async Task<bool> BildirimGonderAsync(int kullaniciId, string baslik, string mesaj, string tip, string? referansTip = null, int? referansId = null)
        {
            var bildirim = new Bildirim
            {
                KullaniciId = kullaniciId,
                Baslik = baslik,
                Mesaj = mesaj,
                Tip = tip,
                ReferansTip = referansTip,
                ReferansId = referansId,
                Okundu = false,
                OlusturmaTarihi = DateTime.UtcNow
            };

            await _unitOfWork.Bildirimler.AddAsync(bildirim);
            await _unitOfWork.SaveChangesAsync();

            // SignalR ile gerçek zamanlı bildirim gönder
            try
            {
                await _hubContext.Clients.Group($"user_{kullaniciId}")
                    .SendAsync("ReceiveNotification", new
                    {
                        id = bildirim.Id,
                        baslik = bildirim.Baslik,
                        mesaj = bildirim.Mesaj,
                        tip = bildirim.Tip,
                        referansTip = bildirim.ReferansTip,
                        referansId = bildirim.ReferansId,
                        olusturmaTarihi = bildirim.OlusturmaTarihi
                    });
            }
            catch { }

            // Push Notification gönder
            try
            {
                await _pushNotificationService.SendNotificationAsync(
                    kullaniciId,
                    baslik,
                    mesaj,
                    new Dictionary<string, string>
                    {
                        { "type", tip },
                        { "referenceType", referansTip ?? "" },
                        { "referenceId", referansId?.ToString() ?? "" }
                    }
                );
            }
            catch { }

            return true;
        }

        public async Task<bool> TopluBildirimGonderAsync(List<int> kullaniciIds, string baslik, string mesaj, string tip)
        {
            var bildirimler = kullaniciIds.Select(kullaniciId => new Bildirim
            {
                KullaniciId = kullaniciId,
                Baslik = baslik,
                Mesaj = mesaj,
                Tip = tip,
                Okundu = false,
                OlusturmaTarihi = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Bildirimler.AddRangeAsync(bildirimler);
            await _unitOfWork.SaveChangesAsync();

            // Her kullanıcıya gerçek zamanlı bildirim gönder
            foreach (var kullaniciId in kullaniciIds)
            {
                try
                {
                    await _hubContext.Clients.Group($"user_{kullaniciId}")
                        .SendAsync("ReceiveNotification", new
                        {
                            baslik = baslik,
                            mesaj = mesaj,
                            tip = tip,
                            olusturmaTarihi = DateTime.UtcNow
                        });
                }
                catch { }
            }

            return true;
        }

        // ... diğer methodlar aynı
        public async Task<IEnumerable<Bildirim>> GetKullaniciBildirimleriAsync(int kullaniciId)
        {
            return await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId);
        }

        public async Task<IEnumerable<Bildirim>> GetOkunmamisBildirimlerAsync(int kullaniciId)
        {
            return await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId && !b.Okundu);
        }

        public async Task<int> GetOkunmamisSayisiAsync(int kullaniciId)
        {
            var okunmamislar = await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId && !b.Okundu);
            return okunmamislar.Count();
        }

        public async Task<bool> OkunduIsaretleAsync(int bildirimId)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(bildirimId);
            if (bildirim == null) return false;

            bildirim.Okundu = true;
            bildirim.OkunmaTarihi = DateTime.UtcNow;

            _unitOfWork.Bildirimler.Update(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TumunuOkunduIsaretleAsync(int kullaniciId)
        {
            var bildirimler = await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId && !b.Okundu);

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
                bildirim.OkunmaTarihi = DateTime.UtcNow;
                _unitOfWork.Bildirimler.Update(bildirim);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BildirimSilAsync(int bildirimId)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(bildirimId);
            if (bildirim == null) return false;

            _unitOfWork.Bildirimler.Remove(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}