using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class DepartmanRepository : Repository<Departman>, IRepository<Departman>
    {
        public DepartmanRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Departman>> GetAktifDepartmanlarAsync()
        {
            return await _context.Departmanlar
                .Where(d => d.Durum)
                .Include(d => d.UstDepartman)
                .OrderBy(d => d.Ad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Departman>> GetAltDepartmanlarAsync(int ustDepartmanId)
        {
            return await _context.Departmanlar
                .Where(d => d.UstDepartmanId == ustDepartmanId)
                .OrderBy(d => d.Ad)
                .ToListAsync();
        }

        public async Task<Departman?> GetWithPersonellerAsync(int id)
        {
            return await _context.Departmanlar
                .Include(d => d.Personeller)
                .Include(d => d.UstDepartman)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> DepartmanKoduVarMiAsync(string kod, int? excludeId = null)
        {
            var query = _context.Departmanlar.Where(d => d.Kod == kod);

            if (excludeId.HasValue)
                query = query.Where(d => d.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}