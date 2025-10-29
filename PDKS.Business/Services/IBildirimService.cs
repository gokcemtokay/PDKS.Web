using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDKS.Business.DTOs;
using PDKS.Data.Entities;

namespace PDKS.Business.Services
{
    public interface IBildirimService
    {
        // Bildirim Okuma (object döner çünkü anonymous type kullanıyor)
        Task<IEnumerable<object>> GetKullaniciBildirimleriAsync(int kullaniciId, bool? sadece_okunmayanlar = null);
        
        // Bildirim İşaretleme
        Task<bool> BildirimOkunduIsaretle(int bildirimId);
        Task<bool> TumBildirimleriOkunduIsaretle(int kullaniciId);
        
        // Bildirim Silme
        Task<bool> BildirimSilAsync(int bildirimId);
        
        // Bildirim Gönderme
        Task<bool> BildirimGonderAsync(int kullaniciId, string baslik, string mesaj, string tip, string? referansTip = null, int? referansId = null);
        Task<bool> TopluBildirimGonderAsync(List<int> kullaniciIds, string baslik, string mesaj, string tip);
        
        // Şirkete Göre Filtreleme
        Task<IEnumerable<BildirimListDTO>> GetBySirketAsync(int sirketId);
    }
}
