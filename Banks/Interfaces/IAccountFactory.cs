using Banks.Models;
using Banks.Models.Accounts;

namespace Banks.Interfaces
{
    public interface IAccountFactory
    {
        DebitAccount OpenDebitAccount(Client client);
        DepositAccount OpenDepositAccount(Client client, int expireDate, double depositAmount);
        CreditAccount OpenCreditAccount(Client client, double limit);
    }
}