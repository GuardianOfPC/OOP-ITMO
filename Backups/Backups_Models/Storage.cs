namespace Backups.Backups_Models
{
    public class Storage
    {
        public Storage(string storageName, string storagePath)
        {
            StorageName = storageName;
            StoragePath = storagePath;
        }

        public string StorageName { get; }
        public string StoragePath { get; }
    }
}