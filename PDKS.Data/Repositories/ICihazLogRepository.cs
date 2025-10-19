using PDKS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Data.Repositories
{
    public interface ICihazLogRepository : IRepository<CihazLog>
    {
        // Not: Bu arayüzdeki metotlar, CihazService'in ihtiyaçlarına göre sadeleştirilmiştir.
        // Eski, kullanılmayan metot tanımları kaldırılmıştır.
        Task<int> GetLogCountByDateAsync(int cihazId, DateTime date);
        Task<int> GetBasariliLogSayisiAsync(int cihazId);
        Task<int> GetBasarisizLogSayisiAsync(int cihazId);
        Task<DateTime?> GetLastLogDateAsync(int cihazId);
    }
}