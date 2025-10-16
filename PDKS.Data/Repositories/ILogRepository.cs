using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface ILogRepository : IRepository<Log>
    {
        Task<IEnumerable<Log>> GetByKullaniciAsync(int kullaniciId);
        Task<IEnumerable<Log>> GetByModulAsync(string modul);
        Task<IEnumerable<Log>> GetByIslemAsync(string islem);
        Task<IEnumerable<Log>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis);
        Task<IEnumerable<Log>> GetRecentLogsAsync(int count = 100);
        Task<IEnumerable<Log>> GetByKullaniciAndDateRangeAsync(int kullaniciId, DateTime baslangic, DateTime bitis);
    }
}