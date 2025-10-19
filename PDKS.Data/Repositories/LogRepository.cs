using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Data.Repositories
{
    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        private readonly PDKSDbContext _context;

        public LogRepository(PDKSDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis)
        {
            return await _context.Loglar
                .Where(l => l.Tarih >= baslangic && l.Tarih <= bitis)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetByKullaniciAsync(int kullaniciId)
        {
            return await _context.Loglar
                .Where(l => l.KullaniciId == kullaniciId)
                .OrderByDescending(l => l.Tarih)
                .ToListAsync();
        }

        public async Task<int> GetErrorLogCountAsync()
        {
            return await _context.Loglar.CountAsync(l => l.LogLevel == "Error");
        }
    }
}