using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;

namespace Banks.Models.Accounts
{
    public class DebitAccount : IAccount, IEquatable<DebitAccount>
    {
        public DebitAccount(Client client, Bank bank)
        {
            Client = client;
            Bank = bank;
        }

        public Client Client { get; }
        public Bank Bank { get; }
        public double Money { get; set; }
        private double InterestAmount { get; set; }
        private List<double> InterestsAmounts { get; } = new ();
        public void WithdrawMoney(int value)
        {
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            if (Money - value < 0) throw new Exception("Couldn't withdraw money - will be broke");
            Money -= value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Withdraw);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((Bank.InterestRate / 365 * 0.01) * Money);
        }

        public void RefillMoney(int value)
        {
            Money += value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Refill);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((Bank.InterestRate / 365 * 0.01) * Money);
        }

        public void TransferMoney(IAccount account, Bank bank, int value)
        {
            if (Money - value < 0) throw new Exception("Couldn't withdraw money - will be broke");
            if (Client.SuspiciousAccountFlag)
            {
                if (value > Bank.TransferLimit) throw new Exception("Transfer limit exceeded");
            }

            Money -= value;
            List<IAccount> accounts = bank.Accounts;
            IAccount neededAccount = accounts.Find(x => x.Equals(account));
            neededAccount.Money += value;
            accounts.Add(neededAccount);
            Bank.CentralBank.BankRepository.UpdateBankAccounts(bank, accounts);
            TransactionLog log = new (this, account, Bank, bank, value, TransactionTypes.Transfer);
            Bank.TransactionLogs.Add(log);
            InterestsAmounts.Add((Bank.InterestRate / 365 * 0.01) * Money);
        }

        public void AddInterest() => Money += CalculateInterestAmount();
        public void ChargeCommission()
        {
            throw new NotImplementedException();
        }

        public bool Equals(DebitAccount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Client, other.Client) && Equals(Bank, other.Bank) && Money.Equals(other.Money);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DebitAccount)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Client, Bank, Money);
        }

        private double CalculateInterestAmount()
        {
            int regularDays = 30 - InterestsAmounts.Count;
            double notRegularInterest = InterestsAmounts.Sum();
            InterestAmount = ((Bank.InterestRate / 365 * 0.01) * Money * regularDays) + notRegularInterest;
            return InterestAmount;
        }
    }
}