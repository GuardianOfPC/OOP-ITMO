using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class Backup
    {
        public LinkedList<RestorePoint> RestorePointsChain { get; set; }
    }
}