using System.Collections.Generic;
using Backups.Interfaces;

namespace Backups.Backups_Models
{
    public class VirtualRepository : IRepository
    {
        public List<Storage> StorageCreation(BackupJob backupJob)
        {
            List<Storage> storages = new ();
            List<JobObject> jobObjects = new (backupJob.JobObjects);
            uint currentRestorePointCount = backupJob.RestorePointsNumber;

            foreach (JobObject jobObject in jobObjects)
            {
                Storage storage = backupJob.StorageMethodStrategy
                    .CreateStorage(jobObject, backupJob.RestorePointsNumber, backupJob.Name);
                storages.Add(storage);
            }

            return storages;
        }
    }
}