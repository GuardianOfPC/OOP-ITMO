using System.Collections.Generic;
using Shops.Models;

namespace Shops.Tools
{
    public class ShopsRepository
    {
        public ShopsRepository() => Shops = new List<Shop>();
        public List<Shop> Shops { get; }

        public void Add(Shop shop)
        {
            Shops.Add(shop);
        }

        public void Remove(Shop shop)
        {
            Shops.Remove(shop);
        }

        public Shop UpdateShopProducts(Shop shop, List<Product> products)
        {
            Shops.Remove(shop);
            shop.ToBuilder()
                .WithProducts(products)
                .Build();
            Shops.Add(shop);
            return shop;
        }

        public bool CheckShop(Shop shop)
        {
            return Shops.Contains(shop);
        }
    }
}