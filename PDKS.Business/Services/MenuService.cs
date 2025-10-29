using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuDto>> GetAllAsync()
        {
            var menuler = await _unitOfWork.Menuler.GetAllAsync();
            return menuler.Select(MapToDto);
        }

        public async Task<MenuDto?> GetByIdAsync(int id)
        {
            var menu = await _unitOfWork.Menuler.GetByIdAsync(id);
            return menu != null ? MapToDto(menu) : null;
        }

        public async Task<IEnumerable<MenuDto>> GetAnaMenulerAsync()
        {
            var anaMenuler = await _unitOfWork.Menuler.GetAnaMenulerAsync();
            var dtoList = new List<MenuDto>();

            foreach (var menu in anaMenuler)
            {
                var dto = MapToDto(menu);
                dto.AltMenuler = (await GetAltMenulerAsync(menu.Id)).ToList();
                dtoList.Add(dto);
            }

            return dtoList;
        }

        private async Task<IEnumerable<MenuDto>> GetAltMenulerAsync(int ustMenuId)
        {
            var altMenuler = await _unitOfWork.Menuler.GetAltMenulerAsync(ustMenuId);
            return altMenuler.Select(MapToDto);
        }

        public async Task<IEnumerable<MenuDto>> GetMenulerByRolIdAsync(int rolId)
        {
            var menuler = await _unitOfWork.Menuler.GetMenulerByRolIdAsync(rolId);

            // Sadece ana menüleri al
            var anaMenuler = menuler.Where(m => m.UstMenuId == null);
            var dtoList = new List<MenuDto>();

            foreach (var menu in anaMenuler)
            {
                var dto = MapToDto(menu);
                // Alt menüleri filtrele (rol yetkisinde olanlar)
                dto.AltMenuler = menuler
                    .Where(m => m.UstMenuId == menu.Id)
                    .Select(MapToDto)
                    .ToList();
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public async Task<MenuDto> CreateAsync(MenuDto dto)
        {
            var menu = new Menu
            {
                MenuAdi = dto.MenuAdi,
                MenuKodu = dto.MenuKodu,
                Url = dto.Url,
                Icon = dto.Icon,
                UstMenuId = dto.UstMenuId,
                Sira = dto.Sira,
                Aktif = dto.Aktif
            };

            await _unitOfWork.Menuler.AddAsync(menu);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(menu);
        }

        public async Task<MenuDto> UpdateAsync(int id, MenuDto dto)
        {
            var menu = await _unitOfWork.Menuler.GetByIdAsync(id);
            if (menu == null)
                throw new KeyNotFoundException("Menü bulunamadı");

            menu.MenuAdi = dto.MenuAdi;
            menu.MenuKodu = dto.MenuKodu;
            menu.Url = dto.Url;
            menu.Icon = dto.Icon;
            menu.UstMenuId = dto.UstMenuId;
            menu.Sira = dto.Sira;
            menu.Aktif = dto.Aktif;

            // ✅ FIX: Update metodu yerine direkt SaveChanges
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(menu);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var menu = await _unitOfWork.Menuler.GetByIdAsync(id);
            if (menu == null)
                return false;

            // ✅ FIX: Delete metodu yerine Remove kullan
            _unitOfWork.Menuler.Remove(menu);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private MenuDto MapToDto(Menu menu)
        {
            return new MenuDto
            {
                Id = menu.Id,
                MenuAdi = menu.MenuAdi,
                MenuKodu = menu.MenuKodu,
                Url = menu.Url,
                Icon = menu.Icon,
                UstMenuId = menu.UstMenuId,
                Sira = menu.Sira,
                Aktif = menu.Aktif
            };
        }

        public async Task<IEnumerable<MenuDto>> GetBySirketAsync(int sirketId)
        {
            var menuler = await _unitOfWork.Menuler.FindAsync(x => x.SirketId == sirketId);
            return menuler.Select(MapToDto);
        }
    }
}
