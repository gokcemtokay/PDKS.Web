using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class BildirimRepository : Repository<Bildirim>, IRepository<Bildirim>
    {
        public BildirimRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Bildirim>> GetByKullaniciAsync(int kullaniciId)
        {
            return await _dbSet
                .Where(b => b.KullaniciId == kullaniciId)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bildirim>> GetOkunmamislarAsync(int kullaniciId)
        {
            return await _dbSet
                .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                .OrderByDescending(b => b.OlusturmaTarihi)
                .ToListAsync();
        }

        public async Task<int> GetOkunmamisSayisiAsync(int kullaniciId)
        {
            return await _dbSet
                .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                .CountAsync();
        }

        public async Task TumunuOkunduIsaretle(int kullaniciId)
        {
            var bildirimler = await _dbSet
                .Where(b => b.KullaniciId == kullaniciId && !b.Okundu)
                .ToListAsync();

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
                bildirim.OkunmaTarihi = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task OkunduIsaretle(int id)
        {
            var bildirim = await _dbSet.FindAsync(id);
            if (bildirim != null && !bildirim.Okundu)
            {
                bildirim.Okundu = true;
                bildirim.OkunmaTarihi = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}