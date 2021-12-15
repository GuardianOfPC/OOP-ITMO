using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Tools
{
    public class DebitAccountFactory : IAccountFactory
    {
        public IAccount OpenAccount(Client client, Bank bank, int expireDate, double depositOrCreditAmount)
        {
            DebitAccount account = new (client, bank);
            List<IAccount> accounts = bank.Accounts;
            accounts.Add(account);
            bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            return account;
        }
    }
}