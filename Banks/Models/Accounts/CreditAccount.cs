using System;
using Banks.Interfaces;

namespace Banks.Models.Accounts
{
    public class CreditAccount : IAccount
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
            Bank.CentralBank.AddLog(log);
        }

        public void RefillMoney(double value)
        {
            Money += value;
            TransactionLog log = new (this, default, Bank, default, value, TransactionTypes.Refill);
            Bank.CentralBank.AddLog(log);
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
            Bank.CentralBank.AddLog(log);
            return log;
        }

        public void AddInterest()
        {
            // Empty by design
        }

        public void ChargeCommission()
        {
            if (!(Money < 0)) return;
            double commissionFinal = (Bank.CommissionRate * 0.01) * Math.Abs(Money);
            Money -= commissionFinal;
        }
    }
}