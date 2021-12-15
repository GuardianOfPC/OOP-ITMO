using Banks.Interfaces;

namespace Banks.Models
{
    public class TransactionLog
    {
        public TransactionLog(IAccount accountFrom, IAccount accountTo, Bank bankFrom, Bank bankTo, double amount)
        {
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            BankFrom = bankFrom;
            BankTo = bankTo;
            Amount = amount;
        }

        public IAccount AccountFrom { get; set; }
        public IAccount AccountTo { get; }
        public Bank BankFrom { get; }
        public Bank BankTo { get; }
        public double Amount { get; }
    }
}