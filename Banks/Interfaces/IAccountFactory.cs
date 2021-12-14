using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Interfaces
{
    public interface IAccountFactory
    {
        DebitAccount OpenDebitAccount(Client client, Bank bank);
        DepositAccount OpenDepositAccount(Client client, Bank bank, int expireDate, double depositAmount);
        CreditAccount OpenCreditAccount(Client client, Bank bank, double limit);
    }
}