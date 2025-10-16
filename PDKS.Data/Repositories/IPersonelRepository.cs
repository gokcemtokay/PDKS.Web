using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IPersonelRepository : IRepository<Personel>
    {
        Task<Personel?> GetBySicilNoAsync(string sicilNo);
        Task<IEnumerable<Personel>> GetAktifPersonellerAsync();
        Task<IEnumerable<Personel>> GetByDepartmanAsync(int departmanId);
        Task<bool> SicilNoVarMiAsync(string sicilNo, int? excludeId = null);
        Task<bool> EmailVarMiAsync(string email, int? excludeId = null);
    }
}