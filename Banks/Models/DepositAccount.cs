using Banks.Interfaces;

namespace Banks.Models
{
    public class DepositAccount : IAccount
    {
        public Client Client { get; }
    }
}