using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class KullaniciRepository : Repository<Kullanici>, IRepository<Kullanici>
    {
        public KullaniciRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<Kullanici?> GetByKullaniciAdiAsync(string kullaniciAdi)
        {
            return await _dbSet
                .Include(k => k.Rol)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi);
        }

        public async Task<Kullanici?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(k => k.Rol)
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(k => k.Email == email);
        }

        public async Task<Kullanici?> GetWithPersonelAsync(int id)
        {
            return await _dbSet
                .Include(k => k.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(k => k.Rol)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kullanici?> GetWithRolAsync(int id)
        {
            return await _dbSet
                .Include(k => k.Rol)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null)
        {
            var query = _dbSet.Where(k => k.KullaniciAdi == kullaniciAdi);

            if (excludeId.HasValue)
                query = query.Where(k => k.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> EmailVarMiAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(k => k.Email == email);

            if (excludeId.HasValue)
                query = query.Where(k => k.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Kullanici>> GetAktifKullanicilarAsync()
        {
            return await _dbSet
                .Include(k => k.Rol)
                .Include(k => k.Personel)
                .Where(k => k.Aktif)
                .OrderBy(k => k.KullaniciAdi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Kullanici>> GetByRolAsync(int rolId)
        {
            return await _dbSet
                .Include(k => k.Personel)
                .Where(k => k.RolId == rolId)
                .OrderBy(k => k.KullaniciAdi)
                .ToListAsync();
        }
    }
}