using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Tools
{
    public class BankRepository : IBankRepository
    {
        private List<Bank> _banks;
        public BankRepository() => _banks = new List<Bank>();

        public void AddBank(Bank bank) => _banks.Add(bank);

        public Bank GetBank(Bank bank)
        {
            return _banks.Find(x => x.Equals(bank));
        }

        public Bank GetBankByName(string name)
        {
            return _banks.Find(x => x.BankName == name);
        }

        public List<Bank> GetBanks()
        {
            return _banks;
        }

        public void UpdateBankAccounts(Bank bank, List<IAccount> accounts)
        {
            Bank neededBank = _banks.Find(x => x.Equals(bank));
            _banks.Remove(neededBank);
            neededBank.Accounts = accounts;
            _banks.Add(neededBank);
        }
    }
}