using System;
using System.Collections.Generic;
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

        public void WithdrawMoney(double value)
        {
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            if (Money <= 0 && Math.Abs(Money - value) > CreditLimit) throw new Exception("Credit limit exceeded");

            Money -= value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Withdraw);
            Bank.TransactionLogs.Add(log);
        }

        public void RefillMoney(double value)
        {
            Money += value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Refill);
            Bank.TransactionLogs.Add(log);
        }

        public TransactionLog TransferMoney(IAccount account, Bank bank, double value)
        {
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            if (Money <= 0 && Math.Abs(Money - value) > CreditLimit) throw new Exception("Credit limit exceeded");
            Money -= value;
            Bank.CentralBank.TransferMoneyAcrossBanks(account, bank, value);
            TransactionLog log = new (this, account, Bank, bank, value, TransactionTypes.Transfer);
            Bank.TransactionLogs.Add(log);
            return log;
        }

        public void AddInterest()
        {
            throw new NotImplementedException();
        }

        public void ChargeCommission()
        {
            if (!(Money < 0)) return;
            double commissionFinal = (Bank.CommissionRate * 0.01) * Math.Abs(Money);
            Money -= commissionFinal;
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
    }
}