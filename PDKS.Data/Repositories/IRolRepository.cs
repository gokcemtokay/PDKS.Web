using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IRolRepository : IRepository<Rol>
    {
        Task<Rol?> GetByRolAdiAsync(string rolAdi);
        Task<IEnumerable<Rol>> GetRollerWithKullaniciSayisiAsync();
        Task<int> GetKullaniciSayisiAsync(int rolId);
    }
}