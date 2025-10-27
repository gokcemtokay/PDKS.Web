using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using PDKS.Data.Repositories;

namespace PDKS.Business.Services
{
    public class RolYetkiService : IRolYetkiService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolYetkiService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<KullaniciYetkiDto> GetKullaniciYetkileriAsync(int kullaniciId)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
            if (kullanici == null)
                throw new KeyNotFoundException("Kullanıcı bulunamadı");

            var result = new KullaniciYetkiDto();

            // Menüleri al
            var menuler = await _unitOfWork.Menuler.GetMenulerByRolIdAsync(kullanici.RolId);
            var anaMenuler = menuler.Where(m => m.UstMenuId == null);

            foreach (var menu in anaMenuler)
            {
                var dto = MapMenuToDto(menu);
                dto.AltMenuler = menuler
                    .Where(m => m.UstMenuId == menu.Id)
                    .Select(MapMenuToDto)
                    .ToList();
                result.Menuler.Add(dto);
            }

            // İşlem yetkilerini al
            var rolYetkiler = await _unitOfWork.RolIslemYetkiler.GetByRolIdAsync(kullanici.RolId);
            result.IslemKodlari = rolYetkiler
                .Where(r => r.Izinli)
                .Select(r => r.IslemYetki.IslemKodu)
                .ToList();

            return result;
        }

        public async Task<RolYetkiAtamaDto> GetRolYetkileriAsync(int rolId)
        {
            var dto = new RolYetkiAtamaDto { RolId = rolId };

            // Menü yetkileri
            var menuRoller = await _unitOfWork.MenuRoller.GetByRolIdAsync(rolId);
            dto.MenuIdler = menuRoller
                .Where(mr => mr.Okuma)
                .Select(mr => mr.MenuId)
                .ToList();

            // İşlem yetkileri
            var rolYetkiler = await _unitOfWork.RolIslemYetkiler.GetByRolIdAsync(rolId);
            dto.IslemYetkiIdler = rolYetkiler
                .Where(r => r.Izinli)
                .Select(r => r.IslemYetkiId)
                .ToList();

            return dto;
        }

        public async Task<bool> RolYetkiAtaAsync(RolYetkiAtamaDto dto)
        {
            // Mevcut yetkileri sil
            await _unitOfWork.MenuRoller.DeleteByRolIdAsync(dto.RolId);
            await _unitOfWork.RolIslemYetkiler.DeleteByRolIdAsync(dto.RolId);

            // Yeni menü yetkilerini ekle
            foreach (var menuId in dto.MenuIdler)
            {
                var menuRol = new MenuRol
                {
                    MenuId = menuId,
                    RolId = dto.RolId,
                    Okuma = true
                };
                await _unitOfWork.MenuRoller.AddAsync(menuRol);
            }

            // Yeni işlem yetkilerini ekle
            foreach (var islemYetkiId in dto.IslemYetkiIdler)
            {
                var rolIslem = new RolIslemYetki
                {
                    RolId = dto.RolId,
                    IslemYetkiId = islemYetkiId,
                    Izinli = true
                };
                await _unitOfWork.RolIslemYetkiler.AddAsync(rolIslem);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPermissionAsync(int kullaniciId, string islemKodu)
        {
            var kullanici = await _unitOfWork.Kullanicilar.GetByIdAsync(kullaniciId);
            if (kullanici == null)
                return false;

            return await _unitOfWork.RolIslemYetkiler.HasPermissionAsync(kullanici.RolId, islemKodu);
        }

        public async Task<IEnumerable<IslemYetkiDto>> GetAllIslemYetkileriAsync()
        {
            var yetkiler = await _unitOfWork.IslemYetkiler.GetAllAsync();
            return yetkiler.Select(y => new IslemYetkiDto
            {
                Id = y.Id,
                IslemKodu = y.IslemKodu,
                IslemAdi = y.IslemAdi,
                ModulAdi = y.ModulAdi,
                Aciklama = y.Aciklama,
                Aktif = y.Aktif
            });
        }

        public async Task<IslemYetkiDto> CreateIslemYetkiAsync(IslemYetkiDto dto)
        {
            var islem = new IslemYetki
            {
                IslemKodu = dto.IslemKodu,
                IslemAdi = dto.IslemAdi,
                ModulAdi = dto.ModulAdi,
                Aciklama = dto.Aciklama,
                Aktif = dto.Aktif
            };

            await _unitOfWork.IslemYetkiler.AddAsync(islem);
            await _unitOfWork.SaveChangesAsync();

            dto.Id = islem.Id;
            return dto;
        }

        private MenuDto MapMenuToDto(Menu menu)
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
    }
}
