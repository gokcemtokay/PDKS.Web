using PDKS.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IIzinService
    {
        Task<IEnumerable<IzinListDTO>> GetAllAsync();
        Task<IzinDetailDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(IzinCreateDTO dto);
        Task UpdateAsync(IzinUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<IzinListDTO>> GetBekleyenIzinlerAsync();
        Task OnaylaReddetAsync(int izinId, string onayDurumu, int onaylayanKullaniciId, string redNedeni); // YENİ EKLENEN SATIR
    }
}