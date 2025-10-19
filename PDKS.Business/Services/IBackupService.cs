using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IBackupService
    {
        Task<string> BackupDatabaseAsync(); // Bu metot adını CreateBackup'tan daha genel hale getirelim.
        Task<bool> RestoreBackup(string backupFilePath);
        Task<IEnumerable<object>> GetBackupHistory(); // object, dosya adı ve tarih gibi bilgileri içerecek.
    }
}