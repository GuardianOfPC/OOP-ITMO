using System;
using System.Collections.Generic;

namespace Backups.Backups_Models
{
    public class RestorePoint
    {
        public DateTime Date { get; set; }
        public List<object> Objects { get; set; }
    }
}