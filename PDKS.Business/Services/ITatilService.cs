using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface ITatilService
    {
        Task<IEnumerable<TatilListDTO>> GetAllAsync();
        Task<TatilListDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(TatilCreateDTO dto);
        Task UpdateAsync(TatilUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<bool> IsTatilAsync(DateTime tarih);
        Task ResmiTatilleriEkleAsync(int yil);
    }
}