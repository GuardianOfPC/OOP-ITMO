using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Tools
{
    public class AccountFactory : IAccountFactory
    {
        public DebitAccount OpenDebitAccount(Client client, Bank bank)
        {
            DebitAccount account = new (client, bank);
            List<IAccount> accounts = bank.Accounts;
            accounts.Add(account);
            bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            return account;
        }

        public DepositAccount OpenDepositAccount(Client client, Bank bank, int expireDate, double depositAmount)
        {
            DepositAccount account = new (client, bank, expireDate, depositAmount);
            List<IAccount> accounts = bank.Accounts;
            accounts.Add(account);
            bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            return account;
        }

        public CreditAccount OpenCreditAccount(Client client, Bank bank, double limit)
        {
            CreditAccount account = new (client, bank, limit);
            List<IAccount> accounts = bank.Accounts;
            accounts.Add(account);
            bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            return account;
        }
    }
}