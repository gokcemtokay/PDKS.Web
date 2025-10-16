using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface ICihazLogRepository : IRepository<CihazLog>
    {
        Task<IEnumerable<CihazLog>> GetByCihazAsync(int cihazId);
        Task<IEnumerable<CihazLog>> GetByCihazAndDateRangeAsync(int cihazId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<CihazLog>> GetBasarisizLoglarAsync(int cihazId);
        Task<int> GetBasarisizLogSayisiAsync(int cihazId, DateTime tarih);
    }
}