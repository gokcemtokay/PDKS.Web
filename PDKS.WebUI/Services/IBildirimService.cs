using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDKS.Data.Entities;

namespace PDKS.WebUI.Services
{
    public interface IBildirimService
    {
        Task<IEnumerable<Bildirim>> GetKullaniciBildirimleriAsync(int kullaniciId);
        Task<IEnumerable<Bildirim>> GetOkunmamisBildirimlerAsync(int kullaniciId);
        Task<int> GetOkunmamisSayisiAsync(int kullaniciId);
        Task<bool> OkunduIsaretleAsync(int bildirimId);
        Task<bool> TumunuOkunduIsaretleAsync(int kullaniciId);
        Task<bool> BildirimSilAsync(int bildirimId);
        Task<bool> BildirimGonderAsync(int kullaniciId, string baslik, string mesaj, string tip, string? referansTip = null, int? referansId = null);
        Task<bool> TopluBildirimGonderAsync(List<int> kullaniciIds, string baslik, string mesaj, string tip);
    }
}
