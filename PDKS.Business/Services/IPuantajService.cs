using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IPuantajService
    {
        // Puantaj Hesaplama
        Task<int> HesaplaPuantajAsync(PuantajHesaplaDTO dto);
        Task<List<int>> TopluPuantajHesaplaAsync(TopluPuantajHesaplaDTO dto);
        Task<PuantajDetailDTO> YenidenHesaplaAsync(int puantajId);

        // CRUD İşlemleri
        Task<PuantajDetailDTO> GetByIdAsync(int id);
        Task<PuantajDetailDTO> GetByPersonelVeDonemAsync(int personelId, int yil, int ay);
        Task<IEnumerable<PuantajListDTO>> GetByDonemAsync(int yil, int ay, int? departmanId = null);
        Task<IEnumerable<PuantajListDTO>> GetByPersonelAsync(int personelId);
        Task<bool> OnaylaAsync(PuantajOnayDTO dto);
        Task<bool> OnayIptalAsync(int puantajId);
        Task<bool> DeleteAsync(int id);

        // Günlük Detaylar
        Task<List<PuantajDetayDTO>> GetGunlukDetaylarAsync(int puantajId);
        Task<PuantajDetayDTO> GetDetayByTarihAsync(int personelId, DateTime tarih);

        // Raporlama
        Task<PuantajOzetRaporDTO> GetOzetRaporAsync(PuantajRaporParametreDTO parametre);
        Task<List<DepartmanPuantajOzetDTO>> GetDepartmanOzetAsync(int yil, int ay);
        Task<byte[]> ExportToExcelAsync(PuantajRaporParametreDTO parametre);

        // Yardımcı Metotlar
        Task<bool> PuantajVarMiAsync(int personelId, int yil, int ay);
        Task<List<string>> ValidasyonKontrolAsync(int personelId, int yil, int ay);
    }
}
