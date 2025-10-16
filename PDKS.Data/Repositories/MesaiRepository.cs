using Microsoft.EntityFrameworkCore;
using PDKS.Data.Context;
using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public class MesaiRepository : GenericRepository<Mesai>, IMesaiRepository
    {
        public MesaiRepository(PDKSDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Mesai>> GetByPersonelAsync(int personelId)
        {
            return await _context.Mesailer
                .Include(m => m.Personel)
                .Include(m => m.OnaylayanKullanici)
                .Where(m => m.PersonelId == personelId)
                .OrderByDescending(m => m.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mesai>> GetByPersonelAndDateRangeAsync(int personelId, DateTime baslangic, DateTime bitis)
        {
            return await _context.Mesailer
                .Include(m => m.Personel)
                .Where(m => m.PersonelId == personelId
                    && m.Tarih.Date >= baslangic.Date
                    && m.Tarih.Date <= bitis.Date)
                .OrderBy(m => m.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mesai>> GetBekleyenMesailerAsync()
        {
            return await _context.Mesailer
                .Include(m => m.Personel)
                .Where(m => m.OnayDurumu == "Beklemede")
                .OrderBy(m => m.Tarih)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mesai>> GetByOnayDurumuAsync(string onayDurumu)
        {
            return await _context.Mesailer
                .Include(m => m.Personel)
                .Include(m => m.OnaylayanKullanici)
                .Where(m => m.OnayDurumu == onayDurumu)
                .OrderByDescending(m => m.Tarih)
                .ToListAsync();
        }

        public async Task<decimal> GetToplamMesaiSaatiAsync(int personelId, int ay, int yil)
        {
            var mesailer = await _context.Mesailer
                .Where(m => m.PersonelId == personelId
                    && m.Tarih.Month == ay
                    && m.Tarih.Year == yil
                    && m.OnayDurumu == "Onaylandı")
                .ToListAsync();

            return mesailer.Sum(m => m.ToplamSaat);
        }
    }
}