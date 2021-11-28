using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Backups_Models;
using Backups.Interfaces;

namespace Backups.Repositories
{
    public class FsRepository : IRepository
    {
        public List<Storage> StorageCreation(BackupJob backupJob)
        {
            List<Storage> storages = new ();

            foreach (JobObject jobObject in backupJob.JobObjects)
            {
                Storage storage = backupJob.StorageMethodStrategy
                    .CreateStorage(jobObject, backupJob.RestorePoints.Count, backupJob.Name);
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