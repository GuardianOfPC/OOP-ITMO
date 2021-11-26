using Backups.Backups_Models;
using Backups.Interfaces;

namespace Backups.Tools
{
    public class SingleStorageStrategy : IStorageMethodStrategy
    {
        public Storage CreateStorage(JobObject jobObject, uint restorePointNumber, string backupJobName)
        {
            Storage storage = new ();
            storage.StorageName = $"{jobObject.FileName}_{restorePointNumber}";
            storage.StoragePath = $"./{backupJobName}/RestorePoint_{restorePointNumber}.zip";
            return storage;
        }
    }
}