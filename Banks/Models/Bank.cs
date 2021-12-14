using System;
using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models.Accounts;

namespace Banks.Models
{
    public class Bank
    {
        public Bank(string name, IAccountFactory accountFactory)
        {
            BankName = name;
            AccountFactory = accountFactory;
        }

        public delegate void AccountHandler();
        public event Client.ClientSubscription DebitInterestRateChanged;
        public event Client.ClientSubscription DepositInterestsRatesChanged;
        public event Client.ClientSubscription CommissionRateChanged;
        public event Client.ClientSubscription TransferLimitChanged;
        public CentralBank CentralBank { get; set; }
        public List<Client> Clients { get; } = new ();
        public List<IAccount> Accounts { get; set; } = new ();
        public List<TransactionLog> TransactionLogs { get; } = new ();
        public IAccountFactory AccountFactory { get; }
        public Dictionary<double, int> DepositInterests { get; private set; }
        public string BankName { get; }
        public double DebitInterestRate { get; private set; }
        public double CommissionRate { get; private set; }
        public double TransferLimit { get; private set; }

        public void SubscribeToChanges()
        {
            CentralBank.AddInterest += AddInterestToAccounts;
            CentralBank.ChargeCommission += ChargeCommissionsFromAccounts;
        }

        public void RegisterClient(Client client) => Clients.Add(client);

        public IAccount OpenAccount(Client client, AccountTypes types, int expirationDate, double amount)
        {
            if (types == AccountTypes.Debit) return AccountFactory.OpenDebitAccount(client, this);
            if (types == AccountTypes.Deposit) return AccountFactory.OpenDepositAccount(client, this, expirationDate, amount);
            if (types == AccountTypes.Credit) return AccountFactory.OpenCreditAccount(client, this, amount);
            return null;
        }

        public void ChangeDebitInterestRate(double value)
        {
            DebitInterestRate = value;
            DebitInterestRateChanged?.Invoke(value, BankPolicyChangeTypes.InterestRate);
        }

        public void ChangeDepositInterestsRate(Dictionary<double, int> set)
        {
            DepositInterests = set;
            foreach ((double rate, int amount) in set)
            {
                DepositInterestsRatesChanged?.Invoke(rate, BankPolicyChangeTypes.DepositInterestRateChanged);
            }
        }

        public void AddInterestToAccounts()
        {
            foreach (IAccount account in Accounts)
            {
                if (account is DebitAccount) account.AddInterest();
                if (account is DepositAccount) account.AddInterest();
            }
        }

        public void ChangeCommissionRate(double value)
        {
            CommissionRate = value;
            CommissionRateChanged?.Invoke(value, BankPolicyChangeTypes.CommissionRate);
        }

        public void ChargeCommissionsFromAccounts()
        {
            foreach (IAccount account in Accounts)
            {
                if (account is CreditAccount) account.ChargeCommission();
            }
        }

        public void ChangeTransferLimit(double value)
        {
            TransferLimit = value;
            TransferLimitChanged?.Invoke(value, BankPolicyChangeTypes.TransferLimit);
        }

        public void CancelTransaction(TransactionLog log, CentralBank centralBank)
        {
            switch (log.Type)
            {
                case TransactionTypes.Withdraw:
                {
                    CancelingHandler(log.AccountFrom, log.BankFrom, log.Amount, centralBank, OperatorTypes.Plus);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                case TransactionTypes.Refill:
                {
                    CancelingHandler(log.AccountFrom, log.BankFrom, log.Amount, centralBank, OperatorTypes.Minus);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                case TransactionTypes.Transfer:
                {
                    CancelingHandler(log.AccountFrom, log.BankFrom, log.Amount, centralBank, OperatorTypes.Plus);
                    CancelingHandler(log.AccountTo, log.BankTo, log.Amount, centralBank, OperatorTypes.Minus);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CancelingHandler(IAccount account, Bank bank, double amount, CentralBank centralBank, OperatorTypes type)
        {
            Bank neededBank = centralBank.BankRepository.GetBank(bank);
            List<IAccount> accounts = neededBank.Accounts;
            IAccount neededAccount = accounts.Find(x => x.Equals(account));
            if (type == OperatorTypes.Plus) neededAccount.Money += amount;
            if (type == OperatorTypes.Minus) neededAccount.Money -= amount;
            accounts.Add(neededAccount);
            centralBank.BankRepository.UpdateBankAccounts(neededBank, accounts);
        }
    }
}