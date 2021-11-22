using System.Collections.Generic;
using Shops.Models;

namespace Shops.Interfaces
{
    public interface IShopService
    {
        IShopsRepository ShopsRepository { get; }
        Shop AddShop(Shop shop);
        Product RegisterProductAtShop(Product product, Shop shop);
        Shop DeliveryToShop(Shop shop, Product product, uint quantity);
        Shop SetProductPrice(Shop shop, Product product, uint price);
        Shop BestPossibleBuy(Dictionary<Product, uint> productsDictionary);
        Customer Buy(Customer customer, Shop shop, Product product, uint quantity);
        Customer MultipleBuy(Customer customer, Shop shop, Dictionary<Product, uint> productsDictionary);
    }
}