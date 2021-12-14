using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;

namespace Banks.Models.Accounts
{
    public class DepositAccount : IAccount, IEquatable<DepositAccount>
    {
        public DepositAccount(Client client, Bank bank, int expirationDate, double depositAmount)
        {
            Client = client;
            Bank = bank;
            ExpirationDate = expirationDate;
            Money = depositAmount;
            foreach ((double rate, int amount) in bank.DepositInterests.Reverse())
            {
                if (depositAmount > amount) continue;
                DepositInterest = rate;
            }
        }

        public Client Client { get; }
        public Bank Bank { get; }
        public double Money { get; set; }
        public int ExpirationDate { get; }
        public double DepositInterest { get; }
        private List<double> InterestsAmounts { get; } = new ();
        public void WithdrawMoney(double value)
        {
            if (Bank.CentralBank.DaysFromCentralBankCreation < ExpirationDate)
                throw new Exception("Cannot withdraw until expiration date");
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            Money -= value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Withdraw);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((DepositInterest / 365 * 0.01) * Money);
        }

        public void RefillMoney(double value)
        {
            Money += value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Refill);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((DepositInterest / 365 * 0.01) * Money);
        }

        public TransactionLog TransferMoney(IAccount account, Bank bank, double value)
        {
            if (Bank.CentralBank.DaysFromCentralBankCreation < ExpirationDate)
                throw new Exception("Cannot withdraw until expiration date");
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            Money -= value;
            Bank.CentralBank.TransferMoneyAcrossBanks(account, bank, value);
            TransactionLog log = new (this, account, Bank, bank, value, TransactionTypes.Transfer);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((DepositInterest / 365 * 0.01) * Money);
            return log;
        }

        public void AddInterest()
        {
            int regularDays = 30 - InterestsAmounts.Count;
            double notRegularInterest = InterestsAmounts.Sum();
            double finalInterest = ((DepositInterest / 365 * 0.01) * Money * regularDays) + notRegularInterest;
            Money += finalInterest;
        }

        public void ChargeCommission()
        {
            throw new NotImplementedException();
        }

        public bool Equals(DepositAccount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Client, other.Client) && Equals(Bank, other.Bank) && Money.Equals(other.Money) && ExpirationDate == other.ExpirationDate && DepositInterest.Equals(other.DepositInterest);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DepositAccount)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Client, Bank, Money, ExpirationDate, DepositInterest);
        }
    }
}