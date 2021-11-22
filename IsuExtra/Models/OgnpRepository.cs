using System.Collections.Generic;
using IsuExtra.Interfaces;

namespace IsuExtra.Models
{
    public class OgnpRepository : IOgnpRepository
    {
        public OgnpRepository() => OgnpList = new List<Ognp>();

        public List<Ognp> OgnpList { get;  }

        public void Add(Ognp ognp) => OgnpList.Add(ognp);
        public void Remove(Ognp ognp) => OgnpList.Remove(ognp);
        public bool CheckOgnp(Ognp ognp) => OgnpList.Contains(ognp);
    }
}