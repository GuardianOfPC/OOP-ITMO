using System.Collections.Generic;
using System.Linq;
using Backups.Backups_Models;
using Backups.Interfaces;

namespace Backups.Repositories
{
    public class VirtualRepository : IRepository
    {
        public List<Storage> StorageCreation(BackupJob backupJob)
        {
            return backupJob.JobObjects.Select(jobObject => backupJob.StorageMethodStrategy.CreateStorage(jobObject, backupJob.RestorePoints.Count, backupJob.Name)).ToList();
        }
    }
}