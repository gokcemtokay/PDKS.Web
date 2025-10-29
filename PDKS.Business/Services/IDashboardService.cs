// PDKS.Business/Services/IDashboardService.cs - YENİ

using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IDashboardService
    {
        // Ana Dashboard
        Task<AnaDashboardDTO> GetAnaDashboardAsync(int kullaniciId);

        // Manager Dashboard
        Task<ManagerDashboardDTO> GetManagerDashboardAsync(int kullaniciId);

        // IK Dashboard
        Task<IKDashboardDTO> GetIKDashboardAsync(int sirketId);

        // Executive Dashboard
        Task<ExecutiveDashboardDTO> GetExecutiveDashboardAsync(int sirketId);

        // Widget Methods
        Task<BugunkunDurumDTO> GetBugunkunDurumAsync(int sirketId);
        Task<List<BekleyenOnayWidgetDTO>> GetBekleyenOnaylarWidgetAsync(int kullaniciId);
        Task<List<SonAktiviteDTO>> GetSonAktivitelerAsync(int kullaniciId, int limit);
        Task<List<DogumGunuDTO>> GetDogumGunleriAsync(int sirketId);
        Task<List<YilDonumuDTO>> GetYilDonumleriAsync(int sirketId);
    
        //Task<IEnumerable<DashboardListDTO>> GetBySirketAsync(int sirketId);
}
}