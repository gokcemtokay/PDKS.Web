using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    // Personel Service Interface
    public interface IPersonelService
    {
        Task<IEnumerable<PersonelListDTO>> GetAllAsync();
        Task<IEnumerable<PersonelListDTO>> GetActivePersonelsAsync();
        Task<PersonelDetailDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(PersonelCreateDTO dto);
        Task UpdateAsync(PersonelUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<PersonelListDTO>> SearchAsync(string searchTerm);
        Task<IEnumerable<PersonelListDTO>> GetByDepartmentAsync(string departman);
        Task<IEnumerable<PersonelListDTO>> GetBySirketAsync(int sirketId);
        Task<int> GetSirketPersonelSayisiAsync(int sirketId);
    }
}
