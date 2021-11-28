using System.IO;
using Backups.Backups_Models;

namespace Backups.Interfaces
{
    public interface IStorageMethodStrategy
    {
        Storage CreateStorage(JobObject jobObject, int restorePointNumber, string backupJobName);
    }
}