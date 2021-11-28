using Banks.Interfaces;
using Banks.Models;

namespace Banks.Tools
{
    public class AccountFactory : IAccountFactory
    {
        private Client Client { get; }
        public DebitAccount CreateDebitAccount()
        {
            throw new System.NotImplementedException();
        }

        public DepositAccount CreateDepositAccount()
        {
            throw new System.NotImplementedException();
        }

        public CreditAccount CreateCreditAccount()
        {
            throw new System.NotImplementedException();
        }
    }
}