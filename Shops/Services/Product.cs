using System;

namespace Shops.Services
{
    public class Product
    {
        private ProductProperty _property = new (1, 1);

        public Product(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            ProductProperty property = new (1, 1);
        }

        public string Name { get; }
        public Guid Id { get; }

        public class ProductProperty
        {
            public ProductProperty(int quantity, int price)
            {
                Quantity = quantity;
                Price = price;
            }

            public int Price { get; private set; }
            public int Quantity { get; private set; }
        }

        // public class ProductBuilder
        // {
        //
        // }
    }
}