using System;
using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class RestorePoint
    {
        public RestorePoint(List<Storage> storages, uint restorePointNumber)
        {
            Date = DateTime.Now;
            Storages = storages;
            RestorePointNumber = restorePointNumber;
        }

        public DateTime Date { get; }
        public List<Storage> Storages { get; }
        public uint RestorePointNumber { get; }
    }
}