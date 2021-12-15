using Banks.Models;

namespace Banks.Interfaces
{
    public interface IAccountFactory
    {
        IAccount OpenAccount(Client client, Bank bank, int expireDate, double depositOrCreditAmount);
    }
}