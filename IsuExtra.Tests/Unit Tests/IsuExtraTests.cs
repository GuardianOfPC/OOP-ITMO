using System.Linq;
using IsuExtra.Models;
using IsuExtra.Services;
using NUnit.Framework;

namespace IsuExtra.Tests.Unit_Tests
{
    public class IsuExtraTests
    {
        private IsuExtraService _isu = new ();

        [Test]
        public void AddOgnpToRegister_OgnpIsInTheRegister()
        {
            Ognp ognp = _isu.AddOgnpToRegister("CyberSec");
            Assert.True(_isu.OgnpRegister.Contains(ognp));
        }
    }
}