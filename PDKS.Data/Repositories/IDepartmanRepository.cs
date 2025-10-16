using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IDepartmanRepository : IRepository<Departman>
    {
        Task<IEnumerable<Departman>> GetAktifDepartmanlarAsync();
        Task<IEnumerable<Departman>> GetAltDepartmanlarAsync(int ustDepartmanId);
        Task<Departman?> GetWithPersonellerAsync(int id);
        Task<bool> DepartmanKoduVarMiAsync(string kod, int? excludeId = null);
    }
}