using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IMesaiService
    {
        Task<IEnumerable<MesaiListDTO>> GetAllAsync();
        Task<MesaiListDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(MesaiCreateDTO dto);
        Task UpdateAsync(MesaiUpdateDTO dto);
        Task DeleteAsync(int id);
        Task OnaylaAsync(int id, int onaylayanKullaniciId);
        Task ReddetAsync(int id, int onaylayanKullaniciId, string redNedeni);
        Task<IEnumerable<MesaiListDTO>> GetBekleyenMesailerAsync();
    
        Task<IEnumerable<MesaiListDTO>> GetBySirketAsync(int sirketId);
}
}