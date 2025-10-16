using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IParametreService
    {
        Task<IEnumerable<ParametreListDTO>> GetAllAsync();
        Task<ParametreListDTO> GetByIdAsync(int id);
        Task<string> GetDegerAsync(string ad);
        Task<int> CreateAsync(ParametreCreateDTO dto);
        Task UpdateAsync(ParametreUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}