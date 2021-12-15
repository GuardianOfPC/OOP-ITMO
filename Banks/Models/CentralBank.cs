using System.Collections.Generic;
using System.Linq;
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
        public List<TransactionLog> TransactionLogs { get; } = new ();
        public void AddLog(TransactionLog log) => TransactionLogs.Add(log);
        public void RemoveLog(TransactionLog log) => TransactionLogs.Remove(log);

        public void ForwardTime(uint value)
        {
            DaysFromCentralBankCreation += value;
            if ((DaysFromCentralBankCreation % 30) == 0)
            {
                ChargeCommission?.Invoke();
                AddInterest?.Invoke();
            }
        }

        public void CancelTransaction(TransactionLog log)
        {
            Bank bankFrom = BankRepository.GetBank(log.BankFrom);
            Bank bankTo = BankRepository.GetBank(log.BankTo);
            TransactionLog neededLog = TransactionLogs.FirstOrDefault(l => l.Equals(log));
            IAccount accountFrom = bankFrom.GetAccount(log.AccountFrom);
            IAccount accountTo = bankTo.GetAccount(log.AccountTo);
            accountFrom.RefillMoney(log.Amount);
            accountTo.WithdrawMoney(log.Amount);
            RemoveLog(neededLog);
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