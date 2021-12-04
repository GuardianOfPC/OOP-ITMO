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
    }
}