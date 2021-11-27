using Banks.Interfaces;

namespace Banks.Models
{
    public class CreditAccount : IAccount
    {
        public Client Client { get; }
    }
}