using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IDepartmanService
    {
        Task<IEnumerable<DepartmanListDTO>> GetAllAsync();
        Task<DepartmanListDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(DepartmanCreateDTO dto);
        Task UpdateAsync(DepartmanUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<DepartmanListDTO>> GetAktifDepartmanlarAsync();
    }
}