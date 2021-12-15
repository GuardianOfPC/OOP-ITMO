using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Tools
{
    public class DepositAccountFactory : IAccountFactory
    {
        public IAccount OpenAccount(Client client, Bank bank, int expireDate, double depositOrCreditAmount)
        {
            DepositAccount account = new (client, bank, expireDate, depositOrCreditAmount);
            List<IAccount> accounts = bank.Accounts;
            accounts.Add(account);
            bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            return account;
        }
    }
}