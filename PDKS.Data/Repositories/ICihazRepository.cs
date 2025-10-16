using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface ICihazRepository : IRepository<Cihaz>
    {
        Task<IEnumerable<Cihaz>> GetAktifCihazlarAsync();
        Task<Cihaz?> GetByIPAdresAsync(string ipAdres);
        Task<int> GetBugunkuOkumaSayisiAsync(int cihazId);
    }
}