using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface ICihazService
    {
        Task<IEnumerable<CihazListDTO>> GetAllAsync();
        Task<CihazUpdateDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(CihazCreateDTO dto);
        Task UpdateAsync(CihazUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<object>> GetCihazLoglariAsync(int cihazId);
    
        Task<IEnumerable<CihazListDTO>> GetBySirketAsync(int sirketId);
}
}