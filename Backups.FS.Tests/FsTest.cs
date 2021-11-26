using Backups.Backups_Models;
using Backups.Tools;

namespace Backups.FS.Tests
{
    internal static class FsTest
    {
        private static void Main()
        {
            BackupJob backupJob = new ("Test", new FsRepository(), new SingleStorageStrategy());
            JobObject jobObject1 = new("file1.txt", "./file1.txt");
            JobObject jobObject2 = new("file2.txt", "./file1.txt");
            
            backupJob.AddJobObject(jobObject1);
            backupJob.AddJobObject(jobObject2);
            backupJob.CreateRestorePoint();
            backupJob.RemoveJobObject(jobObject1);
            backupJob.CreateRestorePoint();
        }
    }
}