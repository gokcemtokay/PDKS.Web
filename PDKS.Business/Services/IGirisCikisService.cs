using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    // GirisCikis Service with Attendance Calculation Logic
    public interface IGirisCikisService
    {
        Task<IEnumerable<GirisCikisListDTO>> GetAllAsync();
        Task<IEnumerable<GirisCikisListDTO>> GetByPersonelAsync(int personelId);
        Task<IEnumerable<GirisCikisListDTO>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis);
        Task<GirisCikisListDTO> GetByIdAsync(int id);
        Task<int> CreateAsync(GirisCikisCreateDTO dto);
        Task UpdateAsync(GirisCikisUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<int> CalculateCalismaSuresi(DateTime? giris, DateTime? cikis);
        Task ProcessGirisCikisAsync(int girisCikisId);
    }
}
