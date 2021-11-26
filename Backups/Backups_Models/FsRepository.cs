using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Backups_Models
{
    public class FsRepository : IRepository
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
                ZipArchive zipArchive = ZipFile
                    .Open(storage.StoragePath, File.Exists(storage.StoragePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create);
                zipArchive.CreateEntryFromFile(jobObject.FilePath, jobObject.FileName);
                zipArchive.Dispose();
                storages.Add(storage);
            }

            return storages;
        }
    }
}