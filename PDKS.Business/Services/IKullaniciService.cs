using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IKullaniciService
    {
        Task<IEnumerable<KullaniciListDTO>> GetAllAsync();
        Task<KullaniciDetailDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(KullaniciCreateDTO dto);
        Task UpdateAsync(KullaniciUpdateDTO dto);
        Task DeleteAsync(int id);
        Task SifreDegistirAsync(int id, string yeniSifre);
        Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null);
    }
}