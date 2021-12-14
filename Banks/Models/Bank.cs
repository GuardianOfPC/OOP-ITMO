﻿using System;
using System.Collections.Generic;
using Banks.Interfaces;
using Banks.Models.Accounts;

namespace Banks.Models
{
    public class Bank : IEquatable<Bank>
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

        public IAccount OpenAccount(Client client, AccountType type, int expirationDate, double amount)
        {
            if (type == AccountType.Debit) return AccountFactory.OpenDebitAccount(client, this);
            if (type == AccountType.Deposit) return AccountFactory.OpenDepositAccount(client, this, expirationDate, amount);
            if (type == AccountType.Credit) return AccountFactory.OpenCreditAccount(client, this, amount);
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
                    Bank neededBank = centralBank.BankRepository.GetBank(log.BankFrom);
                    List<IAccount> accounts = neededBank.Accounts;
                    IAccount neededAccount = accounts.Find(x => x.Equals(log.AccountFrom));
                    neededAccount.Money += log.Amount;
                    accounts.Add(neededAccount);
                    centralBank.BankRepository.UpdateBankAccounts(neededBank, accounts);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                case TransactionTypes.Refill:
                {
                    Bank neededBank = centralBank.BankRepository.GetBank(log.BankFrom);
                    List<IAccount> accounts = neededBank.Accounts;
                    IAccount neededAccount = accounts.Find(x => x.Equals(log.AccountFrom));
                    neededAccount.Money -= log.Amount;
                    accounts.Add(neededAccount);
                    centralBank.BankRepository.UpdateBankAccounts(neededBank, accounts);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                case TransactionTypes.Transfer:
                {
                    Bank bankFrom = centralBank.BankRepository.GetBank(log.BankFrom);
                    List<IAccount> accountsFrom = bankFrom.Accounts;
                    IAccount neededAccountFrom = accountsFrom.Find(x => x.Equals(log.AccountFrom));
                    neededAccountFrom.Money += log.Amount;
                    accountsFrom.Add(neededAccountFrom);
                    centralBank.BankRepository.UpdateBankAccounts(bankFrom, accountsFrom);

                    Bank bankTo = centralBank.BankRepository.GetBank(log.BankTo);
                    List<IAccount> accountsTo = bankTo.Accounts;
                    IAccount neededAccountTo = accountsTo.Find(x => x.Equals(log.AccountTo));
                    neededAccountTo.Money -= log.Amount;
                    accountsTo.Add(neededAccountTo);
                    centralBank.BankRepository.UpdateBankAccounts(bankTo, accountsTo);
                    log.BankFrom.TransactionLogs.Remove(log);
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(Bank other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(
                Clients,
                other.Clients) &&
                   Equals(
                       Accounts,
                       other.Accounts) &&
                   Equals(
                       TransactionLogs,
                       other.TransactionLogs) &&
                   Equals(
                       AccountFactory,
                       other.AccountFactory) &&
                   DebitInterestRate == other.DebitInterestRate &&
                   CommissionRate == other.CommissionRate &&
                   TransferLimit == other.TransferLimit;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Bank)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Clients, Accounts, TransactionLogs, AccountFactory, DebitInterestRate, CommissionRate, TransferLimit);
        }
    }
}