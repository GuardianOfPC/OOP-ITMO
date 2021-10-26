using System;

namespace Shops.Models
{
    public class Product
    {
        private Product(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Id = Guid.NewGuid();
        }

        public string Name { get; }
        public Guid Id { get; }
        public int Price { get; }
        public int Quantity { get; }

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
            private int _price;
            private int _quantity;

            public ProductBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductBuilder WithPrice(int price)
            {
                _price = price;
                return this;
            }

            public ProductBuilder WithQuantity(int quantity)
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