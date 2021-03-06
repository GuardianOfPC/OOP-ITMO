namespace Shops.Models
{
    public class Customer
    {
        private Customer(string name, uint money)
        {
            Name = name;
            Money = money;
        }

        public string Name { get; }
        public uint Money { get; }

        public CustomerBuilder ToBuild()
        {
            CustomerBuilder customerBuilder = new ();
            customerBuilder.WithName(Name);
            customerBuilder.WithMoney(Money);
            customerBuilder.Build();
            return customerBuilder;
        }

        public class CustomerBuilder
        {
            private string _namе;
            private uint _money;

            public CustomerBuilder WithName(string name)
            {
                _namе = name;
                return this;
            }

            public CustomerBuilder WithMoney(uint money)
            {
                _money = money;
                return this;
            }

            public Customer Build()
            {
                Customer finalCustomer = new (_namе, _money);
                return finalCustomer;
            }
        }
    }
}