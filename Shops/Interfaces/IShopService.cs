using System.Collections.Generic;
using Shops.Models;

namespace Shops.Interfaces
{
    public interface IShopService
    {
        public IShopsRepository ShopsRepository { get; }
        public Shop AddShop(Shop shop);
        public Product RegisterProductAtShop(Product product, Shop shop);
        public Shop DeliveryToShop(Shop shop, Product product, uint quantity);
        public Shop SetProductPrice(Shop shop, Product product, uint price);
        public Shop BestPossibleBuy(Product product);
        public Customer Buy(Customer customer, Shop shop, Product product, uint quantity);
        public Customer MultipleBuy(Customer customer, Shop shop, Dictionary<Product, uint> productsDictionary);
    }
}