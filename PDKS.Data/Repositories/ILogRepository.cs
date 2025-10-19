using PDKS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Data.Repositories
{
    public interface ILogRepository : IRepository<Log>
    {
        // Not: Bu arayüz, projenin mevcut ve gelecekteki ihtiyaçlarına göre
        // orijinal projedeki tüm yetenekleri koruyacak şekilde güncellenmiştir.
        Task<IEnumerable<Log>> GetByDateRangeAsync(DateTime baslangic, DateTime bitis);
        Task<IEnumerable<Log>> GetByKullaniciAsync(int kullaniciId);
        Task<int> GetErrorLogCountAsync();
    }
}