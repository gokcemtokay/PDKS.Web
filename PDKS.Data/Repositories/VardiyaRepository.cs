using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class VardiyaRepository : Repository<Vardiya>, IRepository<Vardiya>
    {
        public VardiyaRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vardiya>> GetAktifVardiyalarAsync()
        {
            return await _dbSet
                .Where(v => v.Durum)
                .OrderBy(v => v.Ad)
                .ToListAsync();
        }

        public async Task<Vardiya?> GetByAdAsync(string ad)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.Ad == ad);
        }

        public async Task<IEnumerable<Vardiya>> GetGeceVardiyalariAsync()
        {
            return await _dbSet
                .Where(v => v.GeceVardiyasiMi && v.Durum)
                .OrderBy(v => v.Ad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vardiya>> GetEsnekVardiyalarAsync()
        {
            return await _dbSet
                .Where(v => v.EsnekVardiyaMi && v.Durum)
                .OrderBy(v => v.Ad)
                .ToListAsync();
        }

        public async Task<int> GetPersonelSayisiAsync(int vardiyaId)
        {
            return await _context.Personeller
                .Where(p => p.VardiyaId == vardiyaId)
                .CountAsync();
        }
    }
}