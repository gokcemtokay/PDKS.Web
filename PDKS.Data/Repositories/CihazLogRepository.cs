using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class CihazLogRepository : GenericRepository<CihazLog>, ICihazLogRepository
    {
        public CihazLogRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CihazLog>> GetByCihazAsync(int cihazId)
        {
            return await _dbSet
                .Where(cl => cl.CihazId == cihazId)
                .OrderByDescending(cl => cl.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<CihazLog>> GetByCihazAndDateRangeAsync(int cihazId, DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Where(cl => cl.CihazId == cihazId
                    && cl.Tarih >= baslangic
                    && cl.Tarih <= bitis)
                .OrderByDescending(cl => cl.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<CihazLog>> GetBasarisizLoglarAsync(int cihazId)
        {
            return await _dbSet
                .Where(cl => cl.CihazId == cihazId && !cl.Basarili)
                .OrderByDescending(cl => cl.Tarih)
                .Take(100)
                .ToListAsync();
        }

        public async Task<int> GetBasarisizLogSayisiAsync(int cihazId, DateTime tarih)
        {
            return await _dbSet
                .Where(cl => cl.CihazId == cihazId
                    && !cl.Basarili
                    && cl.Tarih.Date == tarih.Date)
                .CountAsync();
        }
    }
}