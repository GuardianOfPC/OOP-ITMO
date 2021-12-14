using System;
using System.Collections.Generic;
using Banks.Models;
using Banks.Models.Accounts;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests.Unit_Tests
{
    public class BanksTest
    {
        private CentralBank _centralBank;

        [SetUp]
        public void Setup()
        {
            _centralBank = new CentralBank(new BankRepository());
        }

        [Test]
        public void WithdrawFromDebitAccount_MoneyAddedAndExceptionAsserted()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            var account = bank.OpenAccount(client, AccountTypes.Debit, default, default) as DebitAccount;
            account?.RefillMoney(1000);
            Assert.AreEqual(1000,account.Money);
            Assert.Catch<Exception>(() => account.WithdrawMoney(9000));
        } 
        
        [Test]
        public void WithdrawFromSuspiciousDebitAccount_SuspiciousExceptionAsserted()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            bank.ChangeTransferLimit(900);
            var account = bank.OpenAccount(client, AccountTypes.Debit, default, default) as DebitAccount;
            account.RefillMoney(1000);
            Assert.Catch<Exception>(() => account.WithdrawMoney(901));
        }
        
        [Test]
        public void TransferBetweenAccountsAcrossBanksAndCancelTransaction_MoneySentAndReturnedAfterCancel()
        {
            Client client1 = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Client client2 = new Client.ClientBuilder()
                .WithFirstName("John")
                .WithLastName("Doe")
                .WithHomeAddress("LA")
                .WithPassportNumber(81)
                .Build();
            Bank bank1 = new("Sber", new AccountFactory());
            Bank bank2 = new("American Bank", new AccountFactory());
            bank1 = _centralBank.RegisterBank(bank1);
            bank2 = _centralBank.RegisterBank(bank2);
            bank1.RegisterClient(client1);
            bank2.RegisterClient(client2);
            
            var account1 = bank1.OpenAccount(client1, AccountTypes.Debit, default, default) as DebitAccount;
            account1?.RefillMoney(1000);
            
            var account2 = bank2.OpenAccount(client2, AccountTypes.Debit, default, default) as DebitAccount;
            account2?.RefillMoney(1000);
            
            TransactionLog log = account1.TransferMoney(account2, bank2, 500);
            Assert.AreEqual(500,account1.Money);
            Assert.AreEqual(1500,account2.Money);
            bank1.CancelTransaction(log, _centralBank);
            Assert.AreEqual(1000,account1.Money);
            Assert.AreEqual(1000,account2.Money);
        } 
        
        [Test]
        public void DebitAccountInterestCalculation_CorrectAmountWithInterest()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            var account = bank.OpenAccount(client, AccountTypes.Debit, default, default) as DebitAccount;
            bank.ChangeDebitInterestRate(3.65);
            account.RefillMoney(100000);
            Assert.AreEqual(100000,account.Money);
            _centralBank.ForwardTime(30);
            Assert.AreEqual(100300, account.Money);
        }

        [Test]
        public void DepositAccountInterestCalculation_CorrectAmountWithInterest()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            Dictionary<double, int> depoInt = new ()
            {
                {2, 50000},
                {3.65, 100000},
                {5, 200000}
            };
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            bank.ChangeDepositInterestsRate(depoInt);
            var account = bank.OpenAccount(client, AccountTypes.Deposit, 20, 100000);
            _centralBank.ForwardTime(30);
            Assert.AreEqual(100300, account.Money);
        }

        [Test]
        public void CreditAccountCommissionCalculation_CorrectAmountWithCommission()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            bank.ChangeCommissionRate(3);
            var account = bank.OpenAccount(client, AccountTypes.Credit, default, 200);
            account.RefillMoney(100);
            Assert.AreEqual(100,account.Money);
            account.WithdrawMoney(200);
            Assert.AreEqual(-100,account.Money);
            _centralBank.ForwardTime(30);
            Assert.AreEqual(-103,account.Money);
        }

        [Test]
        public void CreditAccountCreditLimitTest_ExceptionCaught()
        {
            Client client = new Client.ClientBuilder()
                .WithFirstName("Alexey")
                .WithLastName("Odinochenko")
                .WithHomeAddress("SPB")
                .WithPassportNumber(228)
                .Build();
            Bank bank = new("Sber", new AccountFactory());
            bank = _centralBank.RegisterBank(bank);
            bank.RegisterClient(client);
            var account = bank.OpenAccount(client, AccountTypes.Credit, default, 200);
            Assert.Catch<Exception>(() => account.WithdrawMoney(201));
        } 
    }
}