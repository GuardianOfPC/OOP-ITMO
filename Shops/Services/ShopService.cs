using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopService
    {
        private readonly List<Shop> _shops = new ();
        private readonly List<Product> _productRegister = new ();

        public IReadOnlyList<Shop> Shops => _shops;

        public Shop AddShop(string name, string address)
        {
            Shop shop = new Shop.ShopBuilder().WithName(name).WithAddress(address).Build();
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name)
        {
            Product product = new (name);
            _productRegister.Add(product);
            return product;
        }

        // public Product Buy(Customer customer, Shop shop, Product product)
        // {
        //     _shops.Remove(shop);
        //     var oldProducts = shop.Products as List<Product>;
        //     if (oldProducts != null)
        //     {
        //         foreach (Product neededProduct in oldProducts)
        //         {
        //             if (product.Id == neededProduct.Id)
        //             {
        //                 // neededProduct._property.Quantity--;
        //                 // customer.Money -=
        //             }
        //         }
        //     }
        // }
    }
}