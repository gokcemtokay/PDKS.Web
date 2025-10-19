using PDKS.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IGirisCikisService
    {
        Task<IEnumerable<GirisCikisListDTO>> GetAllAsync();
        Task<GirisCikisListDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(GirisCikisCreateDTO dto);
        Task UpdateAsync(GirisCikisUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<GirisCikisListDTO>> GetByDateAsync(DateTime date); // YENİ EKLENEN SATIR
    }
}