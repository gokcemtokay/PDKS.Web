using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetAnaMenulerAsync();
        Task<IEnumerable<Menu>> GetAltMenulerAsync(int ustMenuId);
        Task<Menu?> GetByMenuKoduAsync(string menuKodu);
        Task<IEnumerable<Menu>> GetMenulerByRolIdAsync(int rolId);
    }
}
