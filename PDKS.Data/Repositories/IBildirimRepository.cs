using PDKS.Data.Entities;

namespace PDKS.Data.Repositories
{
    public interface IBildirimRepository : IRepository<Bildirim>
    {
        Task<IEnumerable<Bildirim>> GetByKullaniciAsync(int kullaniciId);
        Task<IEnumerable<Bildirim>> GetOkunmamislarAsync(int kullaniciId);
        Task<int> GetOkunmamisSayisiAsync(int kullaniciId);
        Task TumunuOkunduIsaretle(int kullaniciId);
        Task OkunduIsaretle(int id);
    }
}