using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class JobObject
    {
        public JobObject(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }

        public string FileName { get; }
        public string FilePath { get; }
    }
}