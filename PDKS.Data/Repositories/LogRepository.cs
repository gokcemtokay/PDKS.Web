using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        public LogRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Log>> GetByKullaniciAsync(int kullaniciId)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .Where(l => l.KullaniciId == kullaniciId)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetByModulAsync(string modul)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .Where(l => l.Modul == modul)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetByIslemAsync(string islem)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .Where(l => l.Islem == islem)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .Where(l => l.Tarih.Date >= baslangic.Date && l.Tarih.Date <= bitis.Date)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetRecentLogsAsync(int count = 100)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .OrderByDescending(l => l.Tarih)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetByKullaniciAndDateRangeAsync(int kullaniciId, DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(l => l.Kullanici)
                .Where(l => l.KullaniciId == kullaniciId
                    && l.Tarih.Date >= baslangic.Date
                    && l.Tarih.Date <= bitis.Date)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }
    }
}