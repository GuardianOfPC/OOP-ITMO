using System.Collections.Generic;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IBankRepository
    {
        void AddBank(Bank bank);
        Bank GetBank(Bank bank);
        List<Bank> GetBanks();
        Bank GetBankByName(string name);
        void UpdateBankAccounts(Bank bank, List<IAccount> accounts);
    }
}