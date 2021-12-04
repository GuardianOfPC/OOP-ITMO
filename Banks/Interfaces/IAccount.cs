using Banks.Models;

namespace Banks.Interfaces
{
    public interface IAccount
    {
        Client Client { get; }
        int WithdrawMoney();
        int RefillMoney();
        void TransferMoney();
    }
}