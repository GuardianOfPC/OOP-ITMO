using System.Collections.Generic;
using IsuExtra.Models;

namespace IsuExtra.Interfaces
{
    public interface IOgnpRepository
    {
        public List<Ognp> OgnpList { get;  }
        public void Add(Ognp ognp);
        public void Remove(Ognp ognp);
        public bool CheckOgnp(Ognp ognp);
    }
}