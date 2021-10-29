using System.Collections.Generic;
using IsuExtra.Models;

namespace IsuExtra.Services
{
    public class IsuExtraService
    {
        private readonly List<Ognp> _ognpRegister = new ();
        public IReadOnlyCollection<Ognp> OgnpRegister => _ognpRegister;

        public Ognp AddOgnpToRegister(string name)
        {
            Ognp ognp = new Ognp.OgnpBuilder()
                .WithName(name)
                .Build();
            _ognpRegister.Add(ognp);
            return ognp;
        }
    }
}