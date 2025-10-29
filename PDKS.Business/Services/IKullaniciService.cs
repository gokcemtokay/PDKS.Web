using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IKullaniciService
    {
        Task<IEnumerable<KullaniciListDTO>> GetAllAsync();
        Task<KullaniciDetailDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(KullaniciCreateDTO dto);
        Task UpdateAsync(KullaniciUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null);
        Task<bool> EmailVarMiAsync(string email, int? excludeId = null);

        //Task<IEnumerable<KullaniciListDTO>> GetBySirketAsync(int sirketId);
    }
}