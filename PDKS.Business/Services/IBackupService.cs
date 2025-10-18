using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDKS.Business.Services
{
    public interface IBackupService
    {
        Task<bool> CreateBackup(string backupPath);
        Task<bool> RestoreBackup(string backupFile);
        Task<List<BackupInfo>> GetBackupHistory();
    }
}
