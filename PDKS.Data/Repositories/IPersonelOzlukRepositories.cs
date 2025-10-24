using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    // ============================================
    // PERSONEL ÖZLÜK INTERFACE'LERİ
    // IGenericRepository<T> kullanıyor (Delete, DeleteRange)
    // ============================================

    public interface IPersonelAileRepository : IGenericRepository<PersonelAile>
    {
        Task<IEnumerable<PersonelAile>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelAcilDurumRepository : IGenericRepository<PersonelAcilDurum>
    {
        Task<IEnumerable<PersonelAcilDurum>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelSaglikRepository : IGenericRepository<PersonelSaglik>
    {
        Task<PersonelSaglik?> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelEgitimRepository : IGenericRepository<PersonelEgitim>
    {
        Task<IEnumerable<PersonelEgitim>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelIsDeneyimiRepository : IGenericRepository<PersonelIsDeneyimi>
    {
        Task<IEnumerable<PersonelIsDeneyimi>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelDilRepository : IGenericRepository<PersonelDil>
    {
        Task<IEnumerable<PersonelDil>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelSertifikaRepository : IGenericRepository<PersonelSertifika>
    {
        Task<IEnumerable<PersonelSertifika>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelSertifika>> GetExpiringSertifikalarAsync(int daysBeforeExpiry = 30);
        Task<IEnumerable<PersonelSertifika>> GetExpiredSertifikalarAsync();
    }

    public interface IPersonelPerformansRepository : IGenericRepository<PersonelPerformans>
    {
        Task<IEnumerable<PersonelPerformans>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelPerformans>> GetByDonemAsync(string donem);
    }

    public interface IPersonelDisiplinRepository : IGenericRepository<PersonelDisiplin>
    {
        Task<IEnumerable<PersonelDisiplin>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelDisiplin>> GetAktifDisiplinlerAsync(int personelId);
    }

    public interface IPersonelTerfiRepository : IGenericRepository<PersonelTerfi>
    {
        Task<IEnumerable<PersonelTerfi>> GetByPersonelIdAsync(int personelId);
        Task<PersonelTerfi?> GetLastTerfiAsync(int personelId);
    }

    public interface IPersonelUcretDegisiklikRepository : IGenericRepository<PersonelUcretDegisiklik>
    {
        Task<IEnumerable<PersonelUcretDegisiklik>> GetByPersonelIdAsync(int personelId);
        Task<PersonelUcretDegisiklik?> GetLastUcretDegisiklikAsync(int personelId);
    }

    public interface IPersonelReferansRepository : IGenericRepository<PersonelReferans>
    {
        Task<IEnumerable<PersonelReferans>> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelZimmetRepository : IGenericRepository<PersonelZimmet>
    {
        Task<IEnumerable<PersonelZimmet>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelZimmet>> GetAktifZimmetlerAsync(int personelId);
    }

    public interface IPersonelYetkinlikRepository : IGenericRepository<PersonelYetkinlik>
    {
        Task<IEnumerable<PersonelYetkinlik>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelYetkinlik>> GetByYetkinlikTipiAsync(int personelId, string yetkinlikTipi);
    }

    public interface IPersonelEgitimKayitRepository : IGenericRepository<PersonelEgitimKayit>
    {
        Task<IEnumerable<PersonelEgitimKayit>> GetByPersonelIdAsync(int personelId);
        Task<IEnumerable<PersonelEgitimKayit>> GetTamamlananEgitimlerAsync(int personelId);
    }

    public interface IPersonelMaliBilgiRepository : IGenericRepository<PersonelMaliBilgi>
    {
        Task<PersonelMaliBilgi?> GetByPersonelIdAsync(int personelId);
    }

    public interface IPersonelEkBilgiRepository : IGenericRepository<PersonelEkBilgi>
    {
        Task<PersonelEkBilgi?> GetByPersonelIdAsync(int personelId);
    }

}
