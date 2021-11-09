using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Controllers
{
    public interface IShopService
    {
        public ShopsRepository ShopsRepository { get; }
        public Shop AddShop(Shop shop);
        public Product RegisterProductAtShop(Product product, Shop shop);
        public Shop DeliveryToShop(Shop shop, Product product, int quantity);
        public Shop SetProductPrice(Shop shop, Product product, int price);
        public Shop BestPossibleBuy(Product product);
        public Customer Buy(Customer customer, Shop shop, Product product, int quantity);
        public Customer MultipleBuy(Customer customer, Shop shop, Dictionary<Product, int> productsDictionary);
    }
}