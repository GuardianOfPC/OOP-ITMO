using System;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        Client Client { get; }
        Bank Bank { get; }
        double Money { get; set; }
        void WithdrawMoney(double value);
        void RefillMoney(double value);
        TransactionLog TransferMoney(IAccount account, Bank bank, double value);
        void AddInterest();
        void ChargeCommission();
    }
}