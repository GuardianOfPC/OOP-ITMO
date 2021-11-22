using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class BackupJob
    {
        public List<Job> JobObject { get; set; }
        public List<RestorePoint> RestorePoints { get; set; }
        public Storage StorageMethod { get; set; }

        public RestorePoint CreateRestorePoint()
        {
            
        }
    }
}