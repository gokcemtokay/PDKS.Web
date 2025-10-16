using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IGirisCikisRepository : IRepository<GirisCikis>
    {
        Task<IEnumerable<GirisCikis>> GetByPersonelAsync(int personelId);
        Task<IEnumerable<GirisCikis>> GetByPersonelAndDateAsync(int personelId, DateTime tarih);
        Task<IEnumerable<GirisCikis>> GetByPersonelAndDateRangeAsync(int personelId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<GirisCikis>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis);
        Task<GirisCikis?> GetSonGirisAsync(int personelId);
        Task<IEnumerable<GirisCikis>> GetBugunkuGirisCikislarAsync();
        Task<int> GetBugunkuToplamGirisSayisiAsync();
        Task<IEnumerable<GirisCikis>> GetByCihazAsync(int cihazId);
    }
}