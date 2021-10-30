using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops.Controllers;
using Shops.Models;

namespace Shops.Tests
{
    public class ShopTests
    {
        private ShopService _shopService = new();

        [SetUp]
        public void Setup()
        {
            _shopService = new ShopService();
        }

        [Test]
        public void AddShopToList_ListHasShop()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");
            Assert.True(_shopService.Shops.Contains(shop));
        }

        [Test]
        public void AddShopRegisterProductDeliverSome_ShopHasProductWithQuantity()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");
            Product product = _shopService.RegisterProductAtShop("Молоко", shop);
            _shopService.DeliveryToShop(shop, "Молоко", 10);
            var productsList = (List<Product>)shop.Products;
            // Assert.True(productsList.Find(match => match.Name == product.Name).Quantity == 10);
            foreach (Product current in productsList)
            {
                if (current.Name == product.Name)
                {
                    Assert.True(current.Quantity == 10);
                }
            }
        }

        [Test]
        public void ChangePriceToTheProduct_PriceChanged()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");
            Product product = _shopService.RegisterProductAtShop("Молоко", shop);
            _shopService.DeliveryToShop(shop, "Молоко", 10);
            _shopService.SetProductPrice(shop, "Молоко", 100);
            foreach (Product current in shop.Products)
            {
                if (current.Name == product.Name)
                {
                    Assert.True(current.Price == 100);
                }
            }
        }

        [Test]
        public void BestBuySomeProduct_BestShopFound()
        {
            Shop shop1 = _shopService.AddShop("Ozon", "Moscow City");
            Shop shop2 = _shopService.AddShop("5", "Saint-Petersburg");
            Shop shop3 = _shopService.AddShop("Dixy", "Omsk");
            
            _shopService.RegisterProductAtShop("Молоко", shop1);
            _shopService.RegisterProductAtShop("Молоко", shop2);
            _shopService.RegisterProductAtShop("Молоко", shop3);
            
            _shopService.DeliveryToShop(shop1, "Молоко", 10);
            _shopService.SetProductPrice(shop1, "Молоко", 100);
            
            _shopService.DeliveryToShop(shop2, "Молоко", 10);
            _shopService.SetProductPrice(shop2, "Молоко", 90);
            
            _shopService.DeliveryToShop(shop3, "Молоко", 10);
            _shopService.SetProductPrice(shop3, "Молоко",40);

            Shop goodShop = _shopService.BestPossibleBuy("Молоко", 10);
            Assert.True(goodShop.Name == "Dixy");
        }

        [Test]
        public void BuyProductsFromShop_ProductsBoughtCustomerHasUpdatedMoneyCount()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");

            Product product1 = _shopService.RegisterProductAtShop("Молоко", shop);
            _shopService.DeliveryToShop(shop, "Молоко", 10);
            _shopService.SetProductPrice(shop, "Молоко", 100);

            Product product2 = _shopService.RegisterProductAtShop("Сыр", shop);
            _shopService.DeliveryToShop(shop, "Сыр", 5);
            _shopService.SetProductPrice(shop, "Сыр", 120);

            Customer customer = new Customer.CustomerBuilder()
                .WithName("Alexey")
                .WithMoney(1000)
                .Build();

            customer = _shopService.Buy(customer, shop, "Молоко", 5);
            customer = _shopService.Buy(customer, shop, "Сыр", 2);
            foreach (Product current in shop.Products)
            {
                if (product1.Name == current.Name)
                {
                    Assert.True(current.Quantity == 5);
                }

                if (product2.Name == current.Name)
                {
                    Assert.True(current.Quantity == 3);
                }
            }
            Assert.True(customer.Money == 260);
        }

        [Test]
        public void BuyMultipleProductsFromShop_ProductsBoughtCustomerHasChangedMoney()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");

            Product product1 = _shopService.RegisterProductAtShop("Молоко", shop);
            shop = _shopService.DeliveryToShop(shop, "Молоко", 10);
            shop = _shopService.SetProductPrice(shop, "Молоко", 100);

            Product product2 = _shopService.RegisterProductAtShop("Сыр", shop);
            shop = _shopService.DeliveryToShop(shop, "Сыр", 5);
            shop = _shopService.SetProductPrice(shop, "Сыр", 100);

            Dictionary<string, int> productsDictionary = new ();
            productsDictionary.Add(product1.Name, 5);
            productsDictionary.Add(product2.Name, 2);
            
            Customer customer = new Customer.CustomerBuilder()
                .WithName("Alexey")
                .WithMoney(1000)
                .Build();

            customer = _shopService.MultipleBuy(customer, shop, productsDictionary);
            foreach (Product current in shop.Products)
            {
                if (product1.Name == current.Name)
                {
                    Assert.True(current.Quantity == 5);
                }
            }
            Assert.True(customer.Money == 500);
        }
    }
}