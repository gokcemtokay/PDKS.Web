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
        Task<PuantajDetailDTO> GetByPersonelAsync(int personelId, int yil, int ay); // ✅ 3 parametreli overload eklendi
        Task<IEnumerable<PuantajListDTO>> GetByDonemAsync(int yil, int ay, int? departmanId = null);
        Task<IEnumerable<PuantajListDTO>> GetByPersonelAsync(int personelId); // Mevcut 1 parametreli
        Task<IEnumerable<PuantajListDTO>> GetAllAsync(int sirketId, int yil, int ay); // ✅ Eklendi
        Task<bool> OnaylaAsync(PuantajOnayDTO dto);
        Task<bool> OnayIptalAsync(int puantajId);
        Task<bool> DeleteAsync(int id);

        // Puantaj Oluşturma
        Task<int> OlusturAsync(PuantajCreateDTO dto); // ✅ Eklendi
        Task<List<int>> TopluOlusturAsync(PuantajTopluOlusturDTO dto); // ✅ Eklendi

        // Günlük Detaylar
        Task<List<PuantajDetayDTO>> GetGunlukDetaylarAsync(int puantajId);
        Task<PuantajDetayDTO> GetDetayByTarihAsync(int personelId, DateTime tarih);

        // Raporlama
        Task<PuantajOzetRaporDTO> GetOzetRaporAsync(PuantajRaporParametreDTO parametre);
        Task<List<DepartmanPuantajOzetDTO>> GetDepartmanOzetAsync(int yil, int ay);
        Task<byte[]> ExportToExcelAsync(PuantajRaporParametreDTO parametre);

        // İstatistikler
        Task<object> GetIstatistikAsync(int sirketId, int yil, int ay); // ✅ Eklendi

        // Rapor Metotları
        Task<List<GecKalanlarRaporDTO>> GetGecKalanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis); // ✅ Eklendi
        Task<List<ErkenCikanlarRaporDTO>> GetErkenCikanlarRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis); // ✅ Eklendi
        Task<List<FazlaMesaiRaporDTO>> GetFazlaMesaiRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis); // ✅ Eklendi
        Task<List<DevamsizlarRaporDTO>> GetDevamsizlikRaporuAsync(int sirketId, DateTime baslangic, DateTime bitis); // ✅ Eklendi

        // Yardımcı Metotlar
        Task<bool> PuantajVarMiAsync(int personelId, int yil, int ay);
        Task<List<string>> ValidasyonKontrolAsync(int personelId, int yil, int ay);
    }
}