namespace Backups.Backups_Models
{
    public class Storage
    {
        public Storage() { }
        public Storage(string storageName, string storagePath)
        {
            StorageName = storageName;
            StoragePath = storagePath;
        }

        public string StorageName { get; set; }
        public string StoragePath { get; set; }
    }
}