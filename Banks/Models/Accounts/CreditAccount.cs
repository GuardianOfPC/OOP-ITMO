using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;

namespace Banks.Models.Accounts
{
    public class CreditAccount : IAccount, IEquatable<CreditAccount>
    {
        public CreditAccount(Client client, Bank bank, double limit)
        {
            Client = client;
            Bank = bank;
            CreditLimit = limit;
        }

        public Client Client { get; }
        public Bank Bank { get; }
        public double Money { get; set; }
        public double CreditLimit { get; }
        public double CommissionAmount { get; private set; }

        public void WithdrawMoney(int value)
        {
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            if (Money < 0 && Math.Abs(Money - value) > CreditLimit) throw new Exception("Credit limit exceeded");

            Money -= value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Withdraw);
            Bank.TransactionLogs.Add(log);
        }

        public void RefillMoney(int value)
        {
            Money += value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Refill);
            Bank.TransactionLogs.Add(log);
        }

        public void TransferMoney(IAccount account, Bank bank, int value)
        {
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            if (Money < 0 && Math.Abs(Money - value) > CreditLimit) throw new Exception("Credit limit exceeded");
            Money -= value;
            List<IAccount> accounts = bank.Accounts;
            IAccount neededAccount = accounts.Find(x => x.Equals(account));
            neededAccount.Money += value;
            accounts.Add(neededAccount);
            Bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            TransactionLog log = new (this, account, Bank, bank, value, TransactionTypes.Transfer);
            Bank.TransactionLogs.Add(log);
        }

        public void AddInterest()
        {
            throw new NotImplementedException();
        }

        public void ChargeCommission()
        {
            if (Money < 0) Money -= CommissionAmount;
        }

        public bool Equals(CreditAccount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Client, other.Client) && Equals(Bank, other.Bank) && Money.Equals(other.Money) && CreditLimit.Equals(other.CreditLimit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CreditAccount)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Client, Bank, Money, CreditLimit);
        }

        private double CalculateCommission()
        {
            return CommissionAmount = (Bank.CommissionRate * 0.01) * Math.Abs(Money);
        }
    }
}