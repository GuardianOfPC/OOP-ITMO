using Banks.Interfaces;

namespace Banks.Models
{
    public class DepositAccount : IAccount
    {
        public Client Client { get; }
        public int Money { get; }
        public int WithdrawMoney()
        {
            throw new System.NotImplementedException();
        }

        public int RefillMoney()
        {
            throw new System.NotImplementedException();
        }

        public void TransferMoney()
        {
            throw new System.NotImplementedException();
        }
    }
}