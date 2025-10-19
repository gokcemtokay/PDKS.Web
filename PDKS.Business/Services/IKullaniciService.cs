using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IKullaniciService
    {
        Task<IEnumerable<KullaniciListDTO>> GetAllAsync();

        // Dönüş tipini KullaniciUpdateDTO olarak güncelliyoruz, çünkü API'de
        // bir kullanıcıyı güncellemek için bu DTO'yu kullanıyoruz.
        Task<KullaniciUpdateDTO> GetByIdAsync(int id);

        Task<int> CreateAsync(KullaniciCreateDTO dto);
        Task UpdateAsync(KullaniciUpdateDTO dto);
        // DeleteAsync metodu controller'da kullanılmadığı için şimdilik kaldırabiliriz.
        // İhtiyaç olursa tekrar eklenir.
    }
}