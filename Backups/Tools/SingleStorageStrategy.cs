using Backups.Backups_Models;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SingleStorageStrategy : IStorageMethodStrategy
    {
        public Storage CreateStorage(JobObject jobObject, int restorePointNumber, string backupJobName)
        {
            string storageName = $"{jobObject.FileName}_{restorePointNumber}";
            string storagePath = $"./{backupJobName}/RestorePoint_{restorePointNumber}.zip";
            Storage storage = new (storageName, storagePath);
            return storage;
        }
    }
}