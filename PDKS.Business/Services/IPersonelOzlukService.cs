using PDKS.Business.DTOs;

namespace PDKS.Business.Services
{
    public interface IPersonelOzlukService
    {
        // Aile Bilgileri
        Task<IEnumerable<PersonelAileDTO>> GetAileBilgileriAsync(int personelId);
        Task<PersonelAileDTO> GetAileBilgisiByIdAsync(int id);
        Task<PersonelAileDTO> CreateAileBilgisiAsync(PersonelAileCreateDTO dto);
        Task<bool> UpdateAileBilgisiAsync(int id, PersonelAileCreateDTO dto);
        Task<bool> DeleteAileBilgisiAsync(int id);

        // Acil Durum
        Task<IEnumerable<PersonelAcilDurumDTO>> GetAcilDurumBilgileriAsync(int personelId);
        Task<PersonelAcilDurumDTO> GetAcilDurumByIdAsync(int id);
        Task<PersonelAcilDurumDTO> CreateAcilDurumAsync(PersonelAcilDurumCreateDTO dto);
        Task<bool> UpdateAcilDurumAsync(int id, PersonelAcilDurumCreateDTO dto);
        Task<bool> DeleteAcilDurumAsync(int id);

        // Sağlık
        Task<PersonelSaglikDTO?> GetSaglikBilgisiAsync(int personelId);
        Task<PersonelSaglikDTO> CreateOrUpdateSaglikBilgisiAsync(PersonelSaglikDTO dto);

        // Eğitim Geçmişi
        Task<IEnumerable<PersonelEgitimDTO>> GetEgitimGecmisiAsync(int personelId);
        Task<PersonelEgitimDTO> GetEgitimByIdAsync(int id);
        Task<PersonelEgitimDTO> CreateEgitimAsync(PersonelEgitimCreateDTO dto);
        Task<bool> UpdateEgitimAsync(int id, PersonelEgitimCreateDTO dto);
        Task<bool> DeleteEgitimAsync(int id);

        // İş Deneyimi
        Task<IEnumerable<PersonelIsDeneyimiDTO>> GetIsDeneyimleriAsync(int personelId);
        Task<PersonelIsDeneyimiDTO> GetIsDeneyimiByIdAsync(int id);
        Task<PersonelIsDeneyimiDTO> CreateIsDeneyimiAsync(PersonelIsDeneyimiCreateDTO dto);
        Task<bool> UpdateIsDeneyimiAsync(int id, PersonelIsDeneyimiCreateDTO dto);
        Task<bool> DeleteIsDeneyimiAsync(int id);

        // Dil Becerileri
        Task<IEnumerable<PersonelDilDTO>> GetDilBecerileriAsync(int personelId);
        Task<PersonelDilDTO> GetDilByIdAsync(int id);
        Task<PersonelDilDTO> CreateDilAsync(PersonelDilCreateDTO dto);
        Task<bool> UpdateDilAsync(int id, PersonelDilCreateDTO dto);
        Task<bool> DeleteDilAsync(int id);

        // Sertifikalar
        Task<IEnumerable<PersonelSertifikaDTO>> GetSertifikalarAsync(int personelId);
        Task<PersonelSertifikaDTO> GetSertifikaByIdAsync(int id);
        Task<PersonelSertifikaDTO> CreateSertifikaAsync(PersonelSertifikaCreateDTO dto);
        Task<bool> UpdateSertifikaAsync(int id, PersonelSertifikaCreateDTO dto);
        Task<bool> DeleteSertifikaAsync(int id);
        Task<IEnumerable<PersonelSertifikaDTO>> GetExpiringSertifikalarAsync(int daysBeforeExpiry = 30);
        Task<IEnumerable<PersonelSertifikaDTO>> GetExpiredSertifikalarAsync();

        // Performans
        Task<IEnumerable<PersonelPerformansDTO>> GetPerformansKayitlariAsync(int personelId);
        Task<PersonelPerformansDTO> GetPerformansByIdAsync(int id);
        Task<PersonelPerformansDTO> CreatePerformansAsync(PersonelPerformansCreateDTO dto);
        Task<bool> UpdatePerformansAsync(int id, PersonelPerformansCreateDTO dto);
        Task<bool> DeletePerformansAsync(int id);
        Task<bool> OnaylaPerformansAsync(int id, int onaylayanKullaniciId);

        // Disiplin
        Task<IEnumerable<PersonelDisiplinDTO>> GetDisiplinKayitlariAsync(int personelId);
        Task<PersonelDisiplinDTO> GetDisiplinByIdAsync(int id);
        Task<PersonelDisiplinDTO> CreateDisiplinAsync(PersonelDisiplinCreateDTO dto);
        Task<bool> UpdateDisiplinAsync(int id, PersonelDisiplinCreateDTO dto);
        Task<bool> IptalDisiplinAsync(int id, int iptalEdenKullaniciId, string iptalNedeni);

        // Terfi
        Task<IEnumerable<PersonelTerfiDTO>> GetTerfiGecmisiAsync(int personelId);
        Task<PersonelTerfiDTO> GetTerfiByIdAsync(int id);
        Task<PersonelTerfiDTO> CreateTerfiAsync(PersonelTerfiCreateDTO dto);
        Task<bool> UpdateTerfiAsync(int id, PersonelTerfiCreateDTO dto);

        // Ücret Değişiklik
        Task<IEnumerable<PersonelUcretDegisiklikDTO>> GetUcretDegisiklikleriAsync(int personelId);
        Task<PersonelUcretDegisiklikDTO> GetUcretDegisiklikByIdAsync(int id);
        Task<PersonelUcretDegisiklikDTO> CreateUcretDegisiklikAsync(PersonelUcretDegisiklikCreateDTO dto);
        Task<bool> UpdateUcretDegisiklikAsync(int id, PersonelUcretDegisiklikCreateDTO dto);

        // Referans
        Task<IEnumerable<PersonelReferansDTO>> GetReferanslarAsync(int personelId);
        Task<PersonelReferansDTO> GetReferansByIdAsync(int id);
        Task<PersonelReferansDTO> CreateReferansAsync(PersonelReferansCreateDTO dto);
        Task<bool> UpdateReferansAsync(int id, PersonelReferansCreateDTO dto);
        Task<bool> DeleteReferansAsync(int id);

        // Zimmet
        Task<IEnumerable<PersonelZimmetDTO>> GetZimmetlerAsync(int personelId);
        Task<PersonelZimmetDTO> GetZimmetByIdAsync(int id);
        Task<PersonelZimmetDTO> CreateZimmetAsync(PersonelZimmetCreateDTO dto);
        Task<bool> IadeZimmetAsync(int id, int iadeTeslimAlanKullaniciId, DateTime iadeTarihi);
        Task<bool> DeleteZimmetAsync(int id);

        // Yetkinlik
        Task<IEnumerable<PersonelYetkinlikDTO>> GetYetkinliklerAsync(int personelId);
        Task<PersonelYetkinlikDTO> GetYetkinlikByIdAsync(int id);
        Task<PersonelYetkinlikDTO> CreateYetkinlikAsync(PersonelYetkinlikCreateDTO dto);
        Task<bool> UpdateYetkinlikAsync(int id, PersonelYetkinlikCreateDTO dto);
        Task<bool> DeleteYetkinlikAsync(int id);

        // Eğitim Kayıt
        Task<IEnumerable<PersonelEgitimKayitDTO>> GetEgitimKayitlariAsync(int personelId);
        Task<PersonelEgitimKayitDTO> GetEgitimKayitByIdAsync(int id);
        Task<PersonelEgitimKayitDTO> CreateEgitimKayitAsync(PersonelEgitimKayitCreateDTO dto);
        Task<bool> UpdateEgitimKayitAsync(int id, PersonelEgitimKayitCreateDTO dto);
        Task<bool> DeleteEgitimKayitAsync(int id);

        // Mali Bilgi
        Task<PersonelMaliBilgiDTO?> GetMaliBilgiAsync(int personelId);
        Task<PersonelMaliBilgiDTO> CreateOrUpdateMaliBilgiAsync(PersonelMaliBilgiDTO dto);

        // Ek Bilgi
        Task<PersonelEkBilgiDTO?> GetEkBilgiAsync(int personelId);
        Task<PersonelEkBilgiDTO> CreateOrUpdateEkBilgiAsync(PersonelEkBilgiDTO dto);

        // Kombine
        Task<PersonelOzlukDetayDTO> GetPersonelOzlukDetayAsync(int personelId);
    
        Task<IEnumerable<PersonelListDTO>> GetBySirketAsync(int sirketId);
}
}
