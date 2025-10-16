using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IIzinRepository : IRepository<Izin>
    {
        Task<IEnumerable<Izin>> GetByPersonelAsync(int personelId);
        Task<IEnumerable<Izin>> GetBekleyenIzinlerAsync();
        Task<IEnumerable<Izin>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis);
        Task<int> GetKullanilmisIzinGunuAsync(int personelId, int yil);
        Task<bool> IzinCakismaMiAsync(int personelId, DateTime baslangic, DateTime bitis, int? excludeId = null);
    }
}