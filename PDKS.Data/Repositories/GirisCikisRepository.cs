using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class GirisCikisRepository : GenericRepository<GirisCikis>, IGirisCikisRepository
    {
        public GirisCikisRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GirisCikis>> GetByPersonelAsync(int personelId)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.PersonelId == personelId)
                .OrderByDescending(g => g.GirisZamani)
                .ToListAsync();
        }

        public async Task<IEnumerable<GirisCikis>> GetByPersonelAndDateAsync(int personelId, DateTime tarih)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.PersonelId == personelId && g.GirisZamani.Value.Date == tarih.Date)
                .OrderBy(g => g.GirisZamani)
                .ToListAsync();
        }

        public async Task<IEnumerable<GirisCikis>> GetByPersonelAndDateRangeAsync(int personelId, DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.PersonelId == personelId
                    && g.GirisZamani.Value.Date >= baslangic.Date
                    && g.GirisZamani.Value.Date <= bitis.Date)
                .OrderBy(g => g.GirisZamani)
                .ToListAsync();
        }

        public async Task<IEnumerable<GirisCikis>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.GirisZamani.Value.Date >= baslangic.Date
                    && g.GirisZamani.Value.Date <= bitis.Date)
                .OrderByDescending(g => g.GirisZamani)
                .ToListAsync();
        }

        public async Task<GirisCikis?> GetSonGirisAsync(int personelId)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.PersonelId == personelId)
                .OrderByDescending(g => g.GirisZamani)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GirisCikis>> GetBugunkuGirisCikislarAsync()
        {
            var bugun = DateTime.UtcNow.Date;
            return await _dbSet
                .Include(g => g.Personel)
                .Include(g => g.Cihaz)
                .Where(g => g.GirisZamani.Value.Date == bugun)
                .OrderByDescending(g => g.GirisZamani)
                .ToListAsync();
        }

        public async Task<int> GetBugunkuToplamGirisSayisiAsync()
        {
            var bugun = DateTime.UtcNow.Date;
            return await _dbSet
                .Where(g => g.GirisZamani.Value.Date == bugun)
                .CountAsync();
        }

        public async Task<IEnumerable<GirisCikis>> GetByCihazAsync(int cihazId)
        {
            return await _dbSet
                .Include(g => g.Personel)
                .Where(g => g.CihazId == cihazId)
                .OrderByDescending(g => g.GirisZamani)
                .Take(100)
                .ToListAsync();
        }
    }
}