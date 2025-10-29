using Microsoft.AspNetCore.SignalR;
using PDKS.Business.DTOs;
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
        private readonly IHubContext<Hub> _hubContext;
        private readonly IPushNotificationService _pushNotificationService;

        public BildirimService(
            IUnitOfWork unitOfWork,
            IHubContext<Hub> hubContext,
            IPushNotificationService pushNotificationService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _pushNotificationService = pushNotificationService;
        }

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
                            baslik,
                            mesaj,
                            tip,
                            olusturmaTarihi = DateTime.UtcNow
                        });
                }
                catch { }
            }

            // Push Notifications gönder
            foreach (var kullaniciId in kullaniciIds)
            {
                try
                {
                    await _pushNotificationService.SendNotificationAsync(
                        kullaniciId,
                        baslik,
                        mesaj,
                        new Dictionary<string, string> { { "type", tip } }
                    );
                }
                catch { }
            }

            return true;
        }

        public async Task<IEnumerable<object>> GetKullaniciBildirimleriAsync(int kullaniciId, bool? sadece_okunmayanlar = null)
        {
            var bildirimler = await _unitOfWork.Bildirimler.FindAsync(b => b.KullaniciId == kullaniciId);

            if (sadece_okunmayanlar.HasValue && sadece_okunmayanlar.Value)
            {
                bildirimler = bildirimler.Where(b => !b.Okundu);
            }

            return bildirimler.OrderByDescending(b => b.OlusturmaTarihi).Select(b => new
            {
                b.Id,
                b.Baslik,
                b.Mesaj,
                b.Tip,
                b.Okundu,
                b.OlusturmaTarihi,
                b.ReferansTip,
                b.ReferansId
            });
        }

        public async Task<bool> BildirimOkunduIsaretle(int bildirimId)
        {
            var bildirim = await _unitOfWork.Bildirimler.GetByIdAsync(bildirimId);
            if (bildirim == null) return false;

            bildirim.Okundu = true;
            bildirim.OkunmaTarihi = DateTime.UtcNow;

            _unitOfWork.Bildirimler.Update(bildirim);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TumBildirimleriOkunduIsaretle(int kullaniciId)
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

        public async Task<IEnumerable<BildirimListDTO>> GetBySirketAsync(int sirketId)
        {
            var kullaniciSirketler = await _unitOfWork.KullaniciSirketler
                .FindAsync(ks => ks.SirketId == sirketId && ks.Aktif);

            var kullaniciIds = kullaniciSirketler.Select(ks => ks.KullaniciId).ToList();

            if (!kullaniciIds.Any())
                return Enumerable.Empty<BildirimListDTO>();

            var bildirimler = await _unitOfWork.Bildirimler
                .FindAsync(b => kullaniciIds.Contains(b.KullaniciId));

            return bildirimler.Select(b => new BildirimListDTO
            {
                Id = b.Id,
                KullaniciId = b.KullaniciId,
                Baslik = b.Baslik,
                Mesaj = b.Mesaj,
                Tip = b.Tip,
                Okundu = b.Okundu,
                OlusturmaTarihi = b.OlusturmaTarihi,
                OkunmaTarihi = b.OkunmaTarihi,
                Link = b.Link,
                ReferansTip = b.ReferansTip,
                ReferansId = b.ReferansId
            }).OrderByDescending(b => b.OlusturmaTarihi);
        }
    }
}
