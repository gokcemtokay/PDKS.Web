using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class MenuRolRepository : Repository<MenuRol>, IMenuRolRepository
    {
        public MenuRolRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuRol>> GetByRolIdAsync(int rolId)
        {
            return await _dbSet
                .Include(mr => mr.Menu)
                .Where(mr => mr.RolId == rolId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuRol>> GetByMenuIdAsync(int menuId)
        {
            return await _dbSet
                .Include(mr => mr.Rol)
                .Where(mr => mr.MenuId == menuId)
                .ToListAsync();
        }

        public async Task DeleteByRolIdAsync(int rolId)
        {
            var menuRoller = await _dbSet.Where(mr => mr.RolId == rolId).ToListAsync();
            RemoveRange(menuRoller); // ✅ FIX: _dbSet.RemoveRange yerine RemoveRange
        }
    }

    public class IslemYetkiRepository : Repository<IslemYetki>, IIslemYetkiRepository
    {
        public IslemYetkiRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IslemYetki?> GetByIslemKoduAsync(string islemKodu)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.IslemKodu == islemKodu);
        }

        public async Task<IEnumerable<IslemYetki>> GetByModulAdiAsync(string modulAdi)
        {
            return await _dbSet
                .Where(i => i.ModulAdi == modulAdi && i.Aktif)
                .OrderBy(i => i.IslemAdi)
                .ToListAsync();
        }
    }

    public class RolIslemYetkiRepository : Repository<RolIslemYetki>, IRolIslemYetkiRepository
    {
        public RolIslemYetkiRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RolIslemYetki>> GetByRolIdAsync(int rolId)
        {
            return await _dbSet
                .Include(r => r.IslemYetki)
                .Where(r => r.RolId == rolId && r.Izinli)
                .ToListAsync();
        }

        public async Task<RolIslemYetki?> GetByRolAndIslemAsync(int rolId, int islemYetkiId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.RolId == rolId && r.IslemYetkiId == islemYetkiId);
        }

        public async Task DeleteByRolIdAsync(int rolId)
        {
            var rolYetkiler = await _dbSet.Where(r => r.RolId == rolId).ToListAsync();
            RemoveRange(rolYetkiler); // ✅ FIX: _dbSet.RemoveRange yerine RemoveRange
        }

        public async Task<bool> HasPermissionAsync(int rolId, string islemKodu)
        {
            return await _context.RolIslemYetkiler
                .Include(r => r.IslemYetki)
                .AnyAsync(r => r.RolId == rolId &&
                              r.IslemYetki.IslemKodu == islemKodu &&
                              r.Izinli &&
                              r.IslemYetki.Aktif);
        }
    }
}
