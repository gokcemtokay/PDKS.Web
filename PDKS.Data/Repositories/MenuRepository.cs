using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Menu>> GetAnaMenulerAsync()
        {
            return await _dbSet
                .Where(m => m.UstMenuId == null && m.Aktif)
                .OrderBy(m => m.Sira)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetAltMenulerAsync(int ustMenuId)
        {
            return await _dbSet
                .Where(m => m.UstMenuId == ustMenuId && m.Aktif)
                .OrderBy(m => m.Sira)
                .ToListAsync();
        }

        public async Task<Menu?> GetByMenuKoduAsync(string menuKodu)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.MenuKodu == menuKodu);
        }

        public async Task<IEnumerable<Menu>> GetMenulerByRolIdAsync(int rolId)
        {
            return await _context.MenuRoller
                .Where(mr => mr.RolId == rolId && mr.Okuma && mr.Menu.Aktif)
                .Select(mr => mr.Menu)
                .OrderBy(m => m.Sira)
                .ToListAsync();
        }
    }
}
