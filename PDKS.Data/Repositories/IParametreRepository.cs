using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IParametreRepository : IRepository<Parametre>
    {
        Task<Parametre?> GetByAdAsync(string ad);
        Task<IEnumerable<Parametre>> GetByKategoriAsync(string kategori);
    }
}