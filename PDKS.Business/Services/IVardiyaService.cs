using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IVardiyaService
    {
        Task<IEnumerable<VardiyaListDTO>> GetAllAsync();
        Task<IEnumerable<VardiyaListDTO>> GetAktifVardiyalarAsync();
        Task<VardiyaDetailDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(VardiyaCreateDTO dto);
        Task UpdateAsync(VardiyaUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}