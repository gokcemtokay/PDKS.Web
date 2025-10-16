using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IMesaiRepository : IRepository<Mesai>
    {
        Task<IEnumerable<Mesai>> GetByPersonelAsync(int personelId);
        Task<IEnumerable<Mesai>> GetByPersonelAndDateRangeAsync(int personelId, DateTime baslangic, DateTime bitis);
        Task<IEnumerable<Mesai>> GetBekleyenMesailerAsync();
        Task<IEnumerable<Mesai>> GetByOnayDurumuAsync(string onayDurumu);
        Task<decimal> GetToplamMesaiSaatiAsync(int personelId, int ay, int yil);
    }
}