using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDto>> GetAllAsync();
        Task<MenuDto?> GetByIdAsync(int id);
        Task<IEnumerable<MenuDto>> GetAnaMenulerAsync();
        Task<IEnumerable<MenuDto>> GetMenulerByRolIdAsync(int rolId);
        Task<MenuDto> CreateAsync(MenuDto dto);
        Task<MenuDto> UpdateAsync(int id, MenuDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<MenuDto>> GetBySirketAsync(int sirketId);
    }

    public interface IRolYetkiService
    {
        Task<KullaniciYetkiDto> GetKullaniciYetkileriAsync(int kullaniciId);
        Task<RolYetkiAtamaDto> GetRolYetkileriAsync(int rolId);
        Task<bool> RolYetkiAtaAsync(RolYetkiAtamaDto dto);
        Task<bool> HasPermissionAsync(int kullaniciId, string islemKodu);
        Task<IEnumerable<IslemYetkiDto>> GetAllIslemYetkileriAsync();
        Task<IslemYetkiDto> CreateIslemYetkiAsync(IslemYetkiDto dto);
    }
        
}
