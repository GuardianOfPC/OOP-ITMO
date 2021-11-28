using Banks.Models;

namespace Banks.Interfaces
{
    public interface IAccountFactory
    {
        DebitAccount CreateDebitAccount();
        DepositAccount CreateDepositAccount();
        CreditAccount CreateCreditAccount();
    }
}