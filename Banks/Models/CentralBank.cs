using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Models
{
    public class CentralBank
    {
        public CentralBank(IBankRepository bankRepository)
        {
            BankRepository = bankRepository;
        }

        public event Bank.AccountHandler AddInterest;
        public event Bank.AccountHandler ChargeCommission;
        public IBankRepository BankRepository { get; }
        public uint DaysFromCentralBankCreation { get; private set; }

        public void ForwardTime(uint value)
        {
            DaysFromCentralBankCreation += value;
            if ((DaysFromCentralBankCreation % 30) == 0)
            {
                ChargeCommission?.Invoke();
                AddInterest?.Invoke();
            }
        }

        public void TransferMoneyAcrossBanks(IAccount accountTo, Bank bankTo, double value)
        {
            List<IAccount> accounts = bankTo.Accounts;
            IAccount neededAccount = accounts.Find(x => x.Equals(accountTo));
            accounts.Remove(neededAccount);
            neededAccount.Money += value;
            accounts.Add(neededAccount);
            BankRepository.UpdateBankAccounts(bankTo, accounts);
        }

        public Bank RegisterBank(Bank bank)
        {
            BankRepository.AddBank(bank);
            bank.CentralBank = this;
            bank.SubscribeToChanges();
            return bank;
        }
    }
}