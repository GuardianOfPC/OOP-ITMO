using System;
using System.Collections.Generic;

namespace Shops.Models
{
    public class Shop
    {
        private readonly List<Product> _products;

        private Shop(string name, string address, List<Product> products)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            _products = products;
        }

        public IReadOnlyList<Product> Products => _products;

        public Guid Id { get; }

        public string Name { get; }

        public string Address { get; }

        public ShopBuilder ToBuilder()
        {
            ShopBuilder shopBuilder = new ();
            shopBuilder
                .WithName(Name)
                .WithAddress(Address)
                .WithProducts(_products);
            return shopBuilder;
        }

        public class ShopBuilder
        {
            private List<Product> _products = new ();
            private string _name;
            private string _address;

            public ShopBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ShopBuilder WithAddress(string address)
            {
                _address = address;
                return this;
            }

            public ShopBuilder WithProducts(List<Product> products)
            {
                _products = products;
                return this;
            }

            public Shop Build()
            {
                Shop finalShop = new (_name, _address, _products);
                return finalShop;
            }
        }
    }
}