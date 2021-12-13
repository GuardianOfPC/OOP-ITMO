using System;
using Banks.Models;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        Client Client { get; }
        Bank Bank { get; }
        double Money { get; set; }
        void WithdrawMoney(int value);
        void RefillMoney(int value);
        void TransferMoney(IAccount account, Bank bank, int value);
        void AddInterest();
        void ChargeCommission();
    }
}