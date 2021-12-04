using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Models
{
    public class Bank
    {
        public List<IAccount> Accounts { get; }
        public List<Client> Clients { get; }
        public IAccountFactory AccountFactory { get; }
        public uint InterestRate { get; private set; }
        public uint CommissionRate { get; private set; }
        public uint TransferLimit { get; private set; }

        public delegate void ClientSubscription(uint value);
        public event ClientSubscription InterestRateChanged;
        public event ClientSubscription CommissionRateChanged;
        public event ClientSubscription TransferLimitChanged;

        public void ChangeInterestRate(uint value)
        {
            InterestRate = value;
            InterestRateChanged?.Invoke(value);
        }

        public void ChangeCommissionRate(uint value)
        {
            CommissionRate = value;
            CommissionRateChanged?.Invoke(value);
        }

        public void ChangeTransferLimit(uint value)
        {
            TransferLimit = value;
            TransferLimitChanged?.Invoke(value);
        }
    }
}