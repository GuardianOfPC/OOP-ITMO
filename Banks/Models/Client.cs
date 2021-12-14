﻿using System;
using System.Linq;
using Banks.Models.Accounts;

namespace Banks.Models
{
    public class Client : IEquatable<Client>
    {
        private Client(string firstName, string lastName, string homeAddress, uint passportNumber)
        {
            if (homeAddress == string.Empty || passportNumber == 0) SuspiciousAccountFlag = true;
            else SuspiciousAccountFlag = false;
            FirstName = firstName;
            LastName = lastName;
            HomeAddress = homeAddress;
            PassportNumber = passportNumber;
        }

        public delegate string ClientSubscription(double value, BankPolicyChangeTypes type);

        public string FirstName { get; }

        public string LastName { get; }

        public string HomeAddress { get; }

        public uint PassportNumber { get; }

        public bool SuspiciousAccountFlag { get; }

        public void SubscribeToBankPolicyChanges(Bank bank)
        {
            if (bank.Accounts.Exists(account => account.Client.Equals(this) && account is DebitAccount))
                bank.DebitInterestRateChanged += BankPolicyChangeNotification;
            if (bank.Accounts.Exists(account => account.Client.Equals(this) && account is DepositAccount))
                bank.DepositInterestsRatesChanged += BankPolicyChangeNotification;
            if (bank.Accounts.Exists(account => account.Client.Equals(this) && account is CreditAccount))
                bank.CommissionRateChanged += BankPolicyChangeNotification;
            if (SuspiciousAccountFlag)
                bank.TransferLimitChanged += BankPolicyChangeNotification;
        }

        public string BankPolicyChangeNotification(double value, BankPolicyChangeTypes type)
        {
            string msg = $"${type} changed by {value}";
            return msg;
        }

        public bool Equals(Client other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName && HomeAddress == other.HomeAddress && PassportNumber == other.PassportNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Client)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, HomeAddress, PassportNumber);
        }

        public ClientBuilder ToBuild()
        {
            ClientBuilder builder = new ();
            builder.WithFirstName(FirstName)
                .WithLastName(LastName)
                .WithHomeAddress(HomeAddress)
                .WithPassportNumber(PassportNumber);
            return builder;
        }

        public class ClientBuilder
        {
            private string _firstName;
            private string _lastName;
            private string _homeAddress;
            private uint _passportNumber;

            public ClientBuilder WithFirstName(string firstName)
            {
                _firstName = firstName;
                return this;
            }

            public ClientBuilder WithLastName(string lastName)
            {
                _lastName = lastName;
                return this;
            }

            public ClientBuilder WithHomeAddress(string homeAddress)
            {
                _homeAddress = homeAddress;
                return this;
            }

            public ClientBuilder WithPassportNumber(uint num)
            {
                _passportNumber = num;
                return this;
            }

            public Client Build()
            {
                Client final = new (_firstName, _lastName, _homeAddress, _passportNumber);
                return final;
            }
        }
    }
}