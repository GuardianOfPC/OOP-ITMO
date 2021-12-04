﻿namespace Banks.Models
{
    public class Client
    {
        private Client(string firstName, string lastName, string homeAddress, uint passportNumber)
        {
            if (homeAddress == HomeAddress || passportNumber == PassportNumber) SuspiciousAccountFlag = true;
            else SuspiciousAccountFlag = false;
            FirstName = firstName;
            LastName = lastName;
            HomeAddress = homeAddress;
            PassportNumber = passportNumber;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string HomeAddress { get; }
        public uint PassportNumber { get; }
        public bool SuspiciousAccountFlag { get; }

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