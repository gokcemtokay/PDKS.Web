using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<Rol?> GetByRolAdiAsync(string rolAdi)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.RolAdi == rolAdi);
        }

        public async Task<IEnumerable<Rol>> GetRollerWithKullaniciSayisiAsync()
        {
            return await _dbSet
                .Include(r => r.Kullanicilar)
                .OrderBy(r => r.RolAdi)
                .ToListAsync();
        }

        public async Task<int> GetKullaniciSayisiAsync(int rolId)
        {
            return await _context.Kullanicilar
                .Where(k => k.RolId == rolId)
                .CountAsync();
        }
    }
}