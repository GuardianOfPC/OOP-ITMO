using System.Collections.Generic;
using Shops.Models;

namespace Shops.Interfaces
{
    public interface IShopsRepository
    {
        public List<Shop> Shops { get; }
        public void Add(Shop shop);
        public void Remove(Shop shop);
        public Shop UpdateShopProducts(Shop shop, List<Product> products);
        public bool CheckShop(Shop shop);
    }
}