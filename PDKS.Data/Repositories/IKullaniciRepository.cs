using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IKullaniciRepository : IRepository<Kullanici>
    {
        Task<Kullanici?> GetByKullaniciAdiAsync(string kullaniciAdi);
        Task<Kullanici?> GetByEmailAsync(string email);
        Task<Kullanici?> GetWithPersonelAsync(int id);
        Task<Kullanici?> GetWithRolAsync(int id);
        Task<bool> KullaniciAdiVarMiAsync(string kullaniciAdi, int? excludeId = null);
        Task<bool> EmailVarMiAsync(string email, int? excludeId = null);
        Task<IEnumerable<Kullanici>> GetAktifKullanicilarAsync();
        Task<IEnumerable<Kullanici>> GetByRolAsync(int rolId);
        Task<IEnumerable<Kullanici>> GetAllWithSirketlerAsync();
        Task<Kullanici?> GetByIdWithSirketlerAsync(int id);

    }
}