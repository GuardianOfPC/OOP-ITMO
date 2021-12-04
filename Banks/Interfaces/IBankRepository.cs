using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankRepository
    {
        void AddBank(Bank bank);
    }
}