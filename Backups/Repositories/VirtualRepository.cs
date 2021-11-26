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
            List<JobObject> jobObjects = new (backupJob.JobObjects);

            return jobObjects.Select(jobObject => backupJob.StorageMethodStrategy.CreateStorage(jobObject, backupJob.RestorePointsNumber, backupJob.Name)).ToList();
        }
    }
}