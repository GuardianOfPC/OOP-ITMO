using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class Job
    {
        public string Name { get; set; }
        public List<Storage> Files { get; set; }
    }
}