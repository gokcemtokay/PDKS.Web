// PDKS.Business/Services/IOnayAkisiService.cs - GÜNCELLENMİŞ

using PDKS.Business.DTOs;
using PDKS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IOnayAkisiService
    {
        // Onay Akışı Yönetimi
        Task<IEnumerable<OnayAkisi>> GetAllOnayAkislariAsync(int sirketId);
        Task<OnayAkisi> GetOnayAkisiByIdAsync(int id);
        Task<OnayAkisi> GetOnayAkisiByModulAsync(string modulTipi, int sirketId);
        Task<OnayAkisi> CreateOnayAkisiAsync(OnayAkisiDTO dto);
        Task<OnayAkisi> UpdateOnayAkisiAsync(int id, OnayAkisiDTO dto);
        Task<bool> DeleteOnayAkisiAsync(int id);

        // Talep Onay İşlemleri
        Task<OnayKaydi> BaslatOnayAsync(OnayBaslatDTO dto);
        Task<bool> OnaylaAsync(int onayKaydiId, int kullaniciId, string aciklama);
        Task<bool> ReddetAsync(int onayKaydiId, int kullaniciId, string aciklama);

        // Onay Durumu Sorgulama
        Task<IEnumerable<BekleyenOnayDTO>> GetBekleyenOnaylarAsync(int kullaniciId);
        Task<OnayDurumuDTO> GetOnayDurumuAsync(string modulTipi, int referansId);
        Task<IEnumerable<OnayDurumuDTO>> GetKullaniciTaleplerAsync(int kullaniciId);
    
        Task<IEnumerable<OnayAkisi>> GetBySirketAsync(int sirketId);
}
}