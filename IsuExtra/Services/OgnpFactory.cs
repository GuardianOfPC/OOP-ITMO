using IsuExtra.Interfaces;
using IsuExtra.Models;

namespace IsuExtra.Services
{
    public class OgnpFactory : IOgnpFactory
    {
        private IOgnpRepository _ognpRepository;
        private IIsuExtraService _isuExtraService;

        public IOgnpRepository CreateOgnpRepository()
        {
            return _ognpRepository ??= new OgnpRepository();
        }

        public IIsuExtraService CreateIsuExtraService()
        {
            return _isuExtraService ??= new IsuExtraService(CreateOgnpRepository());
        }
    }
}