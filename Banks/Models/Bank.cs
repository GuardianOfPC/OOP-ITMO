using System.Collections.Generic;
using System.Linq;
using Banks.Interfaces;
using Banks.Models.Accounts;

namespace Banks.Models
{
    public class Bank
    {
        public Bank(string name)
        {
            BankName = name;
        }

        public delegate void AccountHandler();
        public event Client.ClientSubscription DebitInterestRateChanged;
        public event Client.ClientSubscription DepositInterestsRatesChanged;
        public event Client.ClientSubscription CommissionRateChanged;
        public event Client.ClientSubscription TransferLimitChanged;
        public CentralBank CentralBank { get; set; }
        public List<Client> Clients { get; } = new ();
        public List<IAccount> Accounts { get; set; } = new ();
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

        public IAccount GetAccount(IAccount account) => Accounts.FirstOrDefault(a => a.Equals(account));

        public void RegisterClient(Client client) => Clients.Add(client);

        public IAccount OpenAccount(Client client, IAccountFactory factory, int expirationDate, double amount)
        {
            return factory.OpenAccount(client, this, expirationDate, amount);
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
                account.AddInterest();
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
                account.ChargeCommission();
            }
        }

        public void ChangeTransferLimit(double value)
        {
            TransferLimit = value;
            TransferLimitChanged?.Invoke(value, BankPolicyChangeTypes.TransferLimit);
        }
    }
}