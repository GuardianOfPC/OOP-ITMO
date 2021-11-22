using System.Collections.Generic;
using Shops.Models;

namespace Shops.Interfaces
{
    public interface IShopsRepository
    {
        List<Shop> Shops { get; }
        void Add(Shop shop);
        void Remove(Shop shop);
        Shop UpdateShopProducts(Shop shop, List<Product> products);
        bool CheckShop(Shop shop);
    }
}