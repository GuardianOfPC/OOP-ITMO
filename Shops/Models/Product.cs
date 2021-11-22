using System;

namespace Shops.Models
{
    public class Product
    {
        private Product(string name, uint price, uint quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Id = Guid.NewGuid();
        }

        public string Name { get; }
        public Guid Id { get; }
        public uint Price { get; }
        public uint Quantity { get; }

        public ProductBuilder ToBuilder()
        {
            ProductBuilder productBuilder = new ();
            productBuilder.WithName(Name);
            productBuilder.WithPrice(Price);
            productBuilder.WithQuantity(Quantity);
            return productBuilder;
        }

        public class ProductBuilder
        {
            private string _name;
            private uint _price;
            private uint _quantity;

            public ProductBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductBuilder WithPrice(uint price)
            {
                _price = price;
                return this;
            }

            public ProductBuilder WithQuantity(uint quantity)
            {
                _quantity = quantity;
                return this;
            }

            public Product Build()
            {
                Product finalProduct = new (_name, _price, _quantity);
                return finalProduct;
            }
        }
    }
}