using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class PuantajRepository : Repository<Puantaj>, IRepository<Puantaj>
    {
        public PuantajRepository(PDKSDbContext context) : base(context)
        {
        }

        public override async Task<Puantaj?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .Include(p => p.Detaylar)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Puantaj>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .OrderByDescending(p => p.Yil)
                .ThenByDescending(p => p.Ay)
                .ToListAsync();
        }

        public async Task<IEnumerable<Puantaj>> GetByPersonelAsync(int personelId)
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .Where(p => p.PersonelId == personelId)
                .OrderByDescending(p => p.Yil)
                .ThenByDescending(p => p.Ay)
                .ToListAsync();
        }

        public async Task<IEnumerable<Puantaj>> GetByDonemAsync(int yil, int ay)
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .Where(p => p.Yil == yil && p.Ay == ay)
                .OrderBy(p => p.Personel.AdSoyad)
                .ToListAsync();
        }

        public async Task<Puantaj?> GetByPersonelVeDonemAsync(int personelId, int yil, int ay)
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .Include(p => p.Detaylar)
                .FirstOrDefaultAsync(p => p.PersonelId == personelId && p.Yil == yil && p.Ay == ay);
        }

        public async Task<IEnumerable<Puantaj>> GetOnayBekleyenAsync()
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Where(p => p.Durum == "Taslak")
                .OrderBy(p => p.Yil)
                .ThenBy(p => p.Ay)
                .ThenBy(p => p.Personel.AdSoyad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Puantaj>> GetOnaylananAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(p => p.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(p => p.OnaylayanKullanici)
                .Where(p => p.Durum == "Onaylandı" 
                    && p.OnayTarihi.HasValue 
                    && p.OnayTarihi.Value >= baslangic 
                    && p.OnayTarihi.Value <= bitis)
                .OrderBy(p => p.OnayTarihi)
                .ToListAsync();
        }
    }

    public class PuantajDetayRepository : Repository<PuantajDetay>, IRepository<PuantajDetay>
    {
        public PuantajDetayRepository(PDKSDbContext context) : base(context)
        {
        }

        public override async Task<PuantajDetay?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(pd => pd.Personel)
                .Include(pd => pd.Vardiya)
                .Include(pd => pd.GirisCikis)
                .FirstOrDefaultAsync(pd => pd.Id == id);
        }

        public async Task<IEnumerable<PuantajDetay>> GetByPuantajAsync(int puantajId)
        {
            return await _dbSet
                .Include(pd => pd.Personel)
                .Include(pd => pd.Vardiya)
                .Include(pd => pd.GirisCikis)
                .Where(pd => pd.PuantajId == puantajId)
                .OrderBy(pd => pd.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<PuantajDetay>> GetByPersonelVeTarihAralikAsync(
            int personelId, DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(pd => pd.Vardiya)
                .Include(pd => pd.GirisCikis)
                .Where(pd => pd.PersonelId == personelId 
                    && pd.Tarih >= baslangic 
                    && pd.Tarih <= bitis)
                .OrderBy(pd => pd.Tarih)
                .ToListAsync();
        }

        public async Task<PuantajDetay?> GetByPersonelVeTarihAsync(int personelId, DateTime tarih)
        {
            return await _dbSet
                .Include(pd => pd.Personel)
                .Include(pd => pd.Vardiya)
                .Include(pd => pd.GirisCikis)
                .FirstOrDefaultAsync(pd => pd.PersonelId == personelId && pd.Tarih.Date == tarih.Date);
        }

        public async Task<IEnumerable<PuantajDetay>> GetDevamsizlarAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(pd => pd.Personel)
                    .ThenInclude(p => p.Departman)
                .Where(pd => pd.DevamsizMi 
                    && pd.Tarih >= baslangic 
                    && pd.Tarih <= bitis)
                .OrderBy(pd => pd.Tarih)
                .ThenBy(pd => pd.Personel.AdSoyad)
                .ToListAsync();
        }

        public async Task<IEnumerable<PuantajDetay>> GetGecKalmaErkencilanlarAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(pd => pd.Personel)
                    .ThenInclude(p => p.Departman)
                .Include(pd => pd.Vardiya)
                .Where(pd => (pd.GecKaldiMi || pd.ErkenCiktiMi)
                    && pd.Tarih >= baslangic 
                    && pd.Tarih <= bitis)
                .OrderBy(pd => pd.Tarih)
                .ThenBy(pd => pd.Personel.AdSoyad)
                .ToListAsync();
        }
    }
}
