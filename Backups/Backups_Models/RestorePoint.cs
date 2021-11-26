using System;
using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class RestorePoint
    {
        public RestorePoint(List<Storage> storages)
        {
            Date = DateTime.Now;
            Storages = storages;
        }

        public DateTime Date { get; }
        public List<Storage> Storages { get; }
    }
}