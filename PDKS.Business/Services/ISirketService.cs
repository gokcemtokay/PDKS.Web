// PDKS.Business/Services/ISirketService.cs
using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface ISirketService
    {
        // Şirket CRUD
        Task<List<SirketListDTO>> GetAllSirketlerAsync();
        Task<SirketDetailDTO> GetSirketByIdAsync(int id);
        Task<int> CreateSirketAsync(SirketCreateDTO dto);
        Task UpdateSirketAsync(SirketUpdateDTO dto);
        Task DeleteSirketAsync(int id);

        // Ana şirket ve bağlı şirketler
        Task<List<SirketListDTO>> GetAnaSirketlerAsync();
        Task<List<SirketListDTO>> GetBagliSirketlerAsync(int anaSirketId);

        // Personel Transfer
        Task<bool> TransferPersonelAsync(PersonelTransferDTO dto, int kullaniciId);
        Task<List<TransferGecmisiDTO>> GetPersonelTransferGecmisiAsync(int personelId);
        Task<List<TransferGecmisiDTO>> GetSirketTransferGecmisiAsync(int sirketId, DateTime? baslangic = null, DateTime? bitis = null);

        // Konsolide Raporlar
        Task<List<KonsolideRaporDTO>> GetKonsolideRaporAsync(DateTime baslangic, DateTime bitis);
        Task<KonsolideRaporDTO> GetSirketKonsolideRaporAsync(int sirketId, DateTime baslangic, DateTime bitis);

        // İstatistikler
        Task<bool> SirketAktifMiAsync(int sirketId);
        Task<int> GetSirketPersonelSayisiAsync(int sirketId);
    }
}