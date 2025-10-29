using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IParametreService
    {
        Task<List<ParametreDTO>> GetAllAsync();
        Task<ParametreDTO> GetByIdAsync(int id);
        Task<List<ParametreDTO>> GetByKategoriAsync(string kategori); // ⬅️ Kategori kullanıyoruz
        Task<int> CreateAsync(ParametreCreateDTO dto);
        Task UpdateAsync(ParametreUpdateDTO dto);
        Task DeleteAsync(int id);
    
        Task<IEnumerable<ParametreListDTO>> GetBySirketAsync(int sirketId);
}
}