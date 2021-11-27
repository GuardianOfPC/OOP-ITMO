using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Models
{
    public class Bank
    {
        public List<IAccount> Accounts { get; }
        public List<Client> Clients { get; }
    }
}