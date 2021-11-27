using Banks.Interfaces;

namespace Banks.Models
{
    public class DebitAccount : IAccount
    {
        public Client Client { get; }
    }
}