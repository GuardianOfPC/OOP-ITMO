using Backups.Backups_Models;
using Backups.Repositories;
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
            
            BackupJob backupJob1 = new ("Test1", new FsRepository(), new SplitStorageStrategy());
            JobObject jobObject11 = new("file3", "./file3");
            JobObject jobObject22 = new("file4", "./file4");
            
            backupJob1.AddJobObject(jobObject11);
            backupJob1.AddJobObject(jobObject22);
            backupJob1.CreateRestorePoint();
            backupJob1.RemoveJobObject(jobObject11);
            backupJob1.CreateRestorePoint();
        }
    }
}