using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class IzinRepository : GenericRepository<Izin>, IIzinRepository
    {
        public IzinRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Izin>> GetByPersonelAsync(int personelId)
        {
            return await _dbSet
                .Include(i => i.Personel)
                .Where(i => i.PersonelId == personelId)
                .OrderByDescending(i => i.BaslangicTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Izin>> GetBekleyenIzinlerAsync()
        {
            return await _dbSet
                .Include(i => i.Personel)
                .Where(i => i.OnayDurumu == "Beklemede")
                .OrderBy(i => i.BaslangicTarihi)
                .ToListAsync();
        }

        public async Task<IEnumerable<Izin>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis)
        {
            return await _dbSet
                .Include(i => i.Personel)
                .Where(i => i.BaslangicTarihi <= bitis && i.BitisTarihi >= baslangic)
                .OrderBy(i => i.BaslangicTarihi)
                .ToListAsync();
        }

        public async Task<int> GetKullanilmisIzinGunuAsync(int personelId, int yil)
        {
            var izinler = await _dbSet
                .Where(i => i.PersonelId == personelId
                    && i.BaslangicTarihi.Year == yil
                    && i.OnayDurumu == "Onaylandı")
                .ToListAsync();

            return izinler.Sum(i => i.IzinGunSayisi);
        }

        public async Task<bool> IzinCakismaMiAsync(int personelId, DateTime baslangic, DateTime bitis, int? excludeId = null)
        {
            var query = _dbSet.Where(i =>
                i.PersonelId == personelId
                && i.OnayDurumu != "Reddedildi"
                && ((i.BaslangicTarihi <= bitis && i.BitisTarihi >= baslangic)));

            if (excludeId.HasValue)
                query = query.Where(i => i.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}