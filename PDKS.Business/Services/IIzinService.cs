using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IIzinService
    {
        Task<IEnumerable<IzinListDTO>> GetAllAsync();
        Task<IEnumerable<IzinListDTO>> GetBekleyenIzinlerAsync();
        Task<IEnumerable<IzinListDTO>> GetByPersonelAsync(int personelId);
        Task<IzinDetailDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(IzinCreateDTO dto);
        Task UpdateAsync(IzinUpdateDTO dto);
        Task DeleteAsync(int id);
        Task OnaylaAsync(int id, int onaylayanKullaniciId);
        Task ReddetAsync(int id, int onaylayanKullaniciId, string redNedeni);
    }
}