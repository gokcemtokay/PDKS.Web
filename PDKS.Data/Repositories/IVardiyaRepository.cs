using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IVardiyaRepository : IRepository<Vardiya>
    {
        Task<IEnumerable<Vardiya>> GetAktifVardiyalarAsync();
        Task<Vardiya?> GetByAdAsync(string ad);
        Task<IEnumerable<Vardiya>> GetGeceVardiyalariAsync();
        Task<IEnumerable<Vardiya>> GetEsnekVardiyalarAsync();
        Task<int> GetPersonelSayisiAsync(int vardiyaId);
    }
}