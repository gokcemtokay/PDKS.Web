using System;
using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class ParametreRepository : Repository<Parametre>, IRepository<Parametre>
    {
        public ParametreRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<Parametre?> GetByAdAsync(string ad)
        {
            return await _context.Parametreler
                .FirstOrDefaultAsync(p => p.Ad == ad);
        }

        public async Task<IEnumerable<Parametre>> GetByKategoriAsync(string kategori)
        {
            return await _context.Parametreler
                .Where(p => p.Kategori == kategori)
                .OrderBy(p => p.Ad)
                .ToListAsync();
        }
    }
}