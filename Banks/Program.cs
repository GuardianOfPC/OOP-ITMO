using System;
using System.Collections.Generic;
using Banks.Models;
using Banks.Tools;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            CentralBank centralBank = new (new BankRepository());

            Console.WriteLine("\nWelcome to BankApp\n");
            Console.WriteLine("Please choose an option:");
            while (true)
            {
                Console.WriteLine("1. Create bank");
                Console.WriteLine("2. Create client and open an account");
                Console.WriteLine("3. Exit");
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Enter bank name: ");
                        string name = Console.ReadLine();

                        Bank bank = new (name);
                        centralBank.RegisterBank(bank);

                        Console.WriteLine("Enter debit interest rate: ");
                        double debitIntRate = Convert.ToDouble(Console.ReadLine());
                        bank.ChangeDebitInterestRate(debitIntRate);

                        Console.WriteLine("Enter commission rate: ");
                        double commissionRate = Convert.ToDouble(Console.ReadLine());
                        bank.ChangeCommissionRate(commissionRate);

                        Console.WriteLine("Enter transfer limit: ");
                        double transferLimit = Convert.ToDouble(Console.ReadLine());
                        bank.ChangeTransferLimit(transferLimit);

                        Console.WriteLine("Initializing deposit interests configurator\n");
                        Console.WriteLine("Enter ranges count: ");
                        int count = Convert.ToInt32(Console.ReadLine());
                        Dictionary<double, int> depositInterests = new ();
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine("Enter interest rate: ");
                            double rate = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("Enter limit for this rate: ");
                            int limit = Convert.ToInt32(Console.ReadLine());
                            depositInterests.Add(rate, limit);
                        }

                        bank.ChangeDepositInterestsRate(depositInterests);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Enter first name: ");
                        string firstName = Console.ReadLine();

                        Console.WriteLine("Enter last name: ");
                        string lastName = Console.ReadLine();

                        Console.WriteLine("Enter client home address: ");
                        string homeAdress = Console.ReadLine();

                        Console.WriteLine("Enter client passport number");
                        uint passportNum = Convert.ToUInt32(Console.ReadLine());

                        Client client = new Client.ClientBuilder()
                            .WithFirstName(firstName)
                            .WithLastName(lastName)
                            .WithHomeAddress(homeAdress)
                            .WithPassportNumber(passportNum)
                            .Build();
                        Console.WriteLine("Would you like to open an account?");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        string choice2 = Console.ReadLine();
                        switch (choice2)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("Enter bank name: ");
                                Bank ourBank = centralBank.BankRepository.GetBankByName(Console.ReadLine());
                                Console.WriteLine("Available account types:\n");
                                Console.WriteLine("1. Debit");
                                Console.WriteLine("2. Deposit");
                                Console.WriteLine("3. Credit");
                                Console.Write("Enter account type: ");
                                string typeChoice = Console.ReadLine();
                                switch (typeChoice)
                                {
                                    case "1":
                                        ourBank.OpenAccount(client, new DebitAccountFactory(), default, default);
                                        break;
                                    case "2":
                                        Console.WriteLine("Enter expiration date (days): ");
                                        int days = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Enter deposit amount: ");
                                        double amount = Convert.ToDouble(Console.ReadLine());
                                        ourBank.OpenAccount(client, new DepositAccountFactory(), days, amount);
                                        break;
                                    case "3":
                                        Console.WriteLine("Enter credit limit: ");
                                        double limit = Convert.ToDouble(Console.ReadLine());
                                        ourBank.OpenAccount(client, new CreditAccountFactory(), default, limit);
                                        break;
                                    default:
                                        throw new Exception("Invalid argument");
                                }

                                break;
                            case "2":
                                Console.Clear();
                                Console.WriteLine("Okay");
                                Environment.Exit(0);
                                break;
                        }

                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Exit");
                        Environment.Exit(0);
                        break;
                    default:
                        throw new Exception("Invalid argument");
                }
            }
        }
    }
}
