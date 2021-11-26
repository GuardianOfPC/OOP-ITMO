using System.Collections.Generic;
using Backups.Backups_Models;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests.Unit_Tests
{
    public class BackupsTest
    {
        private BackupJob _backupJob;
        
        [SetUp]
        public void Setup()
        {
            _backupJob = new BackupJob("Test", new VirtualRepository(), new SplitStorageStrategy());
        }

        [Test]
        public void CreateFilesAddToJobCreatePoints_RestorePointsAndStoragesAreInTheRightCount()
        {
            JobObject jobObject1 = new("File_A", "Path1");
            JobObject jobObject2 = new("File_B", "Path2");
            _backupJob.AddJobObject(jobObject1);
            _backupJob.AddJobObject(jobObject2);
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveJobObject(jobObject1);
            _backupJob.CreateRestorePoint();
            List<RestorePoint> restorePoints = _backupJob.RestorePoints;
            
            Assert.AreEqual(restorePoints.Count, 2);
            
            List<Storage> firstStorages = restorePoints[0].Storages;
            List<Storage> secondStorages = restorePoints[1].Storages;
            
            Assert.AreEqual(firstStorages.Count, 2);
            Assert.AreEqual(secondStorages.Count, 1);
        }
        
        [Test]
        public void CreateFilesAddToJobCreatePoints_RestorePointsAndStoragesAreInTheRightCount2()
        {
            JobObject jobObject1 = new("File_A", "Path1");
            JobObject jobObject2 = new("File_B", "Path2");
            JobObject jobObject3 = new("File_C", "Path2");
            _backupJob.AddJobObject(jobObject1);
            _backupJob.AddJobObject(jobObject2);
            _backupJob.AddJobObject(jobObject3);
            _backupJob.CreateRestorePoint();
            _backupJob.CreateRestorePoint();
            _backupJob.RemoveJobObject(jobObject3);
            _backupJob.CreateRestorePoint();
            
            List<RestorePoint> restorePoints = _backupJob.RestorePoints;
            Assert.AreEqual(restorePoints.Count, 3);
            
            List<Storage> firstStorages = restorePoints[0].Storages;
            List<Storage> secondStorages = restorePoints[1].Storages;
            List<Storage> thirdStorages = restorePoints[2].Storages;
            
            Assert.AreEqual(firstStorages.Count, 3);
            Assert.AreEqual(secondStorages.Count, 3);
            Assert.AreEqual(thirdStorages.Count, 2);
        }
        
    }
}