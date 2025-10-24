using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDKS.Data.Repositories
{
    public class CihazLogRepository : GenericRepository<CihazLog>, IGenericRepository<CihazLog>
    {
        private readonly PDKSDbContext _context;

        public CihazLogRepository(PDKSDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetLogCountByDateAsync(int cihazId, DateTime date)
        {
            return await _context.CihazLoglari.CountAsync(l => l.CihazId == cihazId && l.Tarih.Date == date.Date);
        }

        public async Task<int> GetBasariliLogSayisiAsync(int cihazId)
        {
            // Projenin mantığına göre "Hata" olmayan logları başarılı kabul ediyoruz.
            return await _context.CihazLoglari.CountAsync(l => l.CihazId == cihazId && l.Tip != "Hata");
        }

        public async Task<int> GetBasarisizLogSayisiAsync(int cihazId)
        {
            return await _context.CihazLoglari.CountAsync(l => l.CihazId == cihazId && l.Tip == "Hata");
        }

        public async Task<DateTime?> GetLastLogDateAsync(int cihazId)
        {
            return await _context.CihazLoglari
                .Where(l => l.CihazId == cihazId)
                .OrderByDescending(l => l.Tarih)
                .Select(l => (DateTime?)l.Tarih)
                .FirstOrDefaultAsync();
        }

        // Arayüzün gerektirdiği ancak CihazService'te doğrudan kullanılmayan
        // metotlar için temel implementasyonlar (Gerekirse doldurulabilir).
        public async Task<IEnumerable<CihazLog>> GetByCihazAsync(int cihazId)
        {
            return await _context.CihazLoglari.Where(l => l.CihazId == cihazId).ToListAsync();
        }

        public async Task<IEnumerable<CihazLog>> GetByCihazAndDateRangeAsync(int cihazId, DateTime baslangic, DateTime bitis)
        {
            return await _context.CihazLoglari
                .Where(l => l.CihazId == cihazId && l.Tarih >= baslangic && l.Tarih <= bitis)
                .ToListAsync();
        }

        public async Task<int> GetBasarisizLogSayisiAsync(int cihazId, DateTime date)
        {
            return await _context.CihazLoglari.CountAsync(l => l.CihazId == cihazId && l.Tip == "Hata" && l.Tarih.Date == date.Date);
        }

        public async Task<IEnumerable<CihazLog>> GetBasarisizLoglarAsync(int cihazId)
        {
            return await _context.CihazLoglari.Where(l => l.CihazId == cihazId && l.Tip == "Hata").ToListAsync();
        }
    }
}