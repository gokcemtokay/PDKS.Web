using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IMenuRolRepository : IRepository<MenuRol>
    {
        Task<IEnumerable<MenuRol>> GetByRolIdAsync(int rolId);
        Task<IEnumerable<MenuRol>> GetByMenuIdAsync(int menuId);
        Task DeleteByRolIdAsync(int rolId);
    }

    public interface IIslemYetkiRepository : IRepository<IslemYetki>
    {
        Task<IslemYetki?> GetByIslemKoduAsync(string islemKodu);
        Task<IEnumerable<IslemYetki>> GetByModulAdiAsync(string modulAdi);
    }

    public interface IRolIslemYetkiRepository : IRepository<RolIslemYetki>
    {
        Task<IEnumerable<RolIslemYetki>> GetByRolIdAsync(int rolId);
        Task<RolIslemYetki?> GetByRolAndIslemAsync(int rolId, int islemYetkiId);
        Task DeleteByRolIdAsync(int rolId);
        Task<bool> HasPermissionAsync(int rolId, string islemKodu);
    }
}
