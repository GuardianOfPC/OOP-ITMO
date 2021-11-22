using Backups.Backups_Models;
using NUnit.Framework;

namespace Backups.Tests.Unit_Tests
{
    public class BackupsTest
    {
        [Test]
        public void TestCase1()
        {
            List<File> files = {FileA_1, FileB_1, FileC_1};
            BackupJob backupJob = new ();
            backupJob.JobObject.Add(files);
            backupJob.Start();
            RestorePoint restorePoint1 = backupJob.GetCurrentRPoint();
            backupJob.Start();
            RestorePoint restorePoint2 = backupJob.GetCurrentRPoint();
            backupJob.JobObject.Remove(FileC);
            backupJob.Start();
            RestorePoint restorePoint3 = backupJob.GetCurrentRPoint();
        }
        
        [Test]
        public void TestCase2()
        {
            List<File> files = {FileA_1, FileB_1};
            BackupJob backupJob = new ();
            backupJob.JobObject.Add(files);
            backupJob.Start();
            RestorePoint restorePoint1 = backupJob.GetCurrentRPoint();
            backupJob.JobObject.Remove(FileB);
            backupJob.Start();
            RestorePoint restorePoint2 = backupJob.GetCurrentRPoint();
            Assert.True(backupJob.RestorePoints.Count == 2);
            Assert.True(backupJob.Storages.Count == 3);
        }
    }
}