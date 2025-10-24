using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class CihazRepository : Repository<Cihaz>, IRepository<Cihaz>
    {
        public CihazRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cihaz>> GetAktifCihazlarAsync()
        {
            return await _dbSet
                .Where(c => c.Durum)
                .OrderBy(c => c.CihazAdi)
                .ToListAsync();
        }

        public async Task<Cihaz?> GetByIPAdresAsync(string ipAdres)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.IPAdres == ipAdres);
        }

        public async Task<int> GetBugunkuOkumaSayisiAsync(int cihazId)
        {
            var bugun = DateTime.UtcNow.Date;
            return await _context.GirisCikislar
                .Where(g => g.CihazId == cihazId && g.GirisZamani.HasValue && g.GirisZamani.Value.Date == bugun)
                .CountAsync();
        }
    }
}