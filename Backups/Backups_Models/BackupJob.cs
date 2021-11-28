using System.Collections.Generic;
using Backups.Interfaces;

namespace Backups.Backups_Models
{
    public class BackupJob
    {
        public BackupJob(string name, IRepository repository, IStorageMethodStrategy methodStrategy)
        {
            Name = name;
            Repository = repository;
            StorageMethodStrategy = methodStrategy;
            JobObjects = new List<JobObject>();
            RestorePoints = new List<RestorePoint>();
        }

        public string Name { get; }
        public List<JobObject> JobObjects { get; }
        public List<RestorePoint> RestorePoints { get; }
        public IStorageMethodStrategy StorageMethodStrategy { get; }
        public IRepository Repository { get; }

        public void CreateRestorePoint()
        {
            List<Storage> storages = Repository.StorageCreation(this);
            RestorePoint point = new (storages);
            RestorePoints.Add(point);
        }

        public int CountRestorePoints() => RestorePoints.Count;
        public void AddJobObject(JobObject jobObject) => JobObjects.Add(jobObject);
        public void RemoveJobObject(JobObject jobObject) => JobObjects.Remove(jobObject);
    }
}