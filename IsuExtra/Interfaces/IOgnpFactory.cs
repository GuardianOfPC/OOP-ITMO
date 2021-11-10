namespace IsuExtra.Interfaces
{
    public interface IOgnpFactory
    {
        public IOgnpRepository CreateOgnpRepository();
        public IIsuExtraService CreateIsuExtraService();
    }
}