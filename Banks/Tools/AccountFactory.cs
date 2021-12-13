using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Tools
{
    public class AccountFactory : IAccountFactory
    {
        public AccountFactory(Bank bank)
        {
            Bank = bank;
        }

        private Bank Bank { get; }
        public DebitAccount OpenDebitAccount(Client client)
        {
            DebitAccount account = new (client, Bank);
            List<IAccount> accounts = Bank.Accounts;
            accounts.Add(account);
            Bank.CentralBank.BankRepository.UpdateBankAccounts(Bank, accounts);
            return account;
        }

        public DepositAccount OpenDepositAccount(Client client, int expireDate, double depositAmount)
        {
            DepositAccount account = new (client, Bank, expireDate, depositAmount);
            List<IAccount> accounts = Bank.Accounts;
            accounts.Add(account);
            Bank.CentralBank.BankRepository.UpdateBankAccounts(Bank, accounts);
            return account;
        }

        public CreditAccount OpenCreditAccount(Client client, double limit)
        {
            CreditAccount account = new (client, Bank, limit);
            List<IAccount> accounts = Bank.Accounts;
            accounts.Add(account);
            Bank.CentralBank.BankRepository.UpdateBankAccounts(Bank, accounts);
            return account;
        }
    }
}