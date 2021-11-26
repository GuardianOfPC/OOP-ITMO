using System.Collections.Generic;
using Backups.Backups_Models;

namespace Backups.Interfaces
{
    public interface IRepository
    {
        List<Storage> StorageCreation(BackupJob backupJob);
    }
}