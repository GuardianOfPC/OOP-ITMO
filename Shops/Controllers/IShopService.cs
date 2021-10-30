using System.Collections.Generic;
using Shops.Models;

namespace Shops.Controllers
{
    public interface IShopService
    {
        public Shop AddShop(string name, string address);
        public Product RegisterProductAtShop(string name, Shop shop);
        public Shop DeliveryToShop(Shop shop, string name, int quantity);
        public Shop SetProductPrice(Shop shop, string name, int price);
        public Shop BestPossibleBuy(string name, int quantity);
        public Customer Buy(Customer customer, Shop shop, string name, int quantity);
        public Customer MultipleBuy(Customer customer, Shop shop, Dictionary<string, int> productsDictionary);
    }
}