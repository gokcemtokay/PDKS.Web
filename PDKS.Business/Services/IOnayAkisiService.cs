using PDKS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IOnayAkisiService
    {
        Task<IEnumerable<OnayAkisi>> GetBekleyenOnaylarAsync(int personelId);
        Task<bool> OnaylaAsync(int onayAkisiId, bool onaylandi, string aciklama, int onaylayiciId);
        Task<IEnumerable<OnayAkisi>> GetOnayGecmisiAsync(string onayTipi, int referansId);
    }
}