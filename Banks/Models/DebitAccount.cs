using Banks.Interfaces;

namespace Banks.Models
{
    public class DebitAccount : IAccount
    {
        public Client Client { get; }
        public int Money { get; }

        public int WithdrawMoney()
        {
            
        }

        public int RefillMoney()
        {
            
        }

        public void TransferMoney()
        {
            
        }
    }
}