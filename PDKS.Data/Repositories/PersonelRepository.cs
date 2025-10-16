using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class PersonelRepository : GenericRepository<Personel>, IPersonelRepository
    {
        public PersonelRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<Personel?> GetBySicilNoAsync(string sicilNo)
        {
            return await _dbSet
                .Include(p => p.Departman)
                .Include(p => p.Vardiya)
                .FirstOrDefaultAsync(p => p.SicilNo == sicilNo);
        }

        public async Task<IEnumerable<Personel>> GetAktifPersonellerAsync()
        {
            return await _dbSet
                .Include(p => p.Departman)
                .Include(p => p.Vardiya)
                .Where(p => p.Durum)
                .OrderBy(p => p.AdSoyad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Personel>> GetByDepartmanAsync(int departmanId)
        {
            return await _dbSet
                .Include(p => p.Vardiya)
                .Where(p => p.DepartmanId == departmanId)
                .OrderBy(p => p.AdSoyad)
                .ToListAsync();
        }

        public async Task<bool> SicilNoVarMiAsync(string sicilNo, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.SicilNo == sicilNo);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> EmailVarMiAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.Email == email);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}