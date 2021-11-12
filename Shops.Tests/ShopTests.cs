using System.Collections.Generic;
using NUnit.Framework;
using Shops.Interfaces;
using Shops.Models;
using Shops.Services;

namespace Shops.Tests
{
    public class ShopTests
    {
        private IShopService _shopService;

        [SetUp]
        public void Setup()
        {
            IShopFactory shopFactory = new ShopsFactory();
            _shopService = shopFactory.CreateShopService();
        }

        [Test]
        public void AddShopToList_ListHasShop()
        {
            Shop shop = new Shop.ShopBuilder()
                .WithName("Ozon")
                .WithAddress("Moscow City")
                .Build();
            _shopService.AddShop(shop);
            Assert.True(_shopService.ShopsRepository.CheckShop(shop));
        }
        
        [Test]
        public void AddShopRegisterProductDeliverSome_ShopHasProductWithQuantity()
        {
            Shop ozon = new Shop.ShopBuilder()
                .WithName("Ozon")
                .WithAddress("Moscow City")
                .Build();
            Product beer = new Product.ProductBuilder()
                .WithName("Пиво")
                .Build();
            _shopService.RegisterProductAtShop(beer, ozon);
            _shopService.DeliveryToShop(ozon, beer, 10);
            var productsList = (List<Product>)ozon.Products;
            foreach (Product current in productsList)
            {
                if (current.Name == beer.Name) 
                    Assert.True(current.Quantity == 10);
            }
        }
        
        [Test]
        public void ChangePriceToTheProduct_PriceChanged()
        {
            Shop shop = new Shop.ShopBuilder()
                .WithName("Ozon")
                .WithAddress("Moscow City")
                .Build();
            Product product = new Product.ProductBuilder()
                .WithName("Молоко")
                .WithPrice(100)
                .Build();
            _shopService.RegisterProductAtShop(product, shop);
            _shopService.SetProductPrice(shop, product, 100);
            foreach (Product current in shop.Products)
            {
                if (current.Name == product.Name)
                    Assert.True(current.Price == 100);
            }
        }

          [Test]
          public void BestBuySomeProduct_BestShopFound()
          {
              Shop shop1 = new Shop.ShopBuilder()
                  .WithName("Ozon")
                  .WithAddress("Moscow City")
                  .Build();
              Shop shop2 = new Shop.ShopBuilder()
                  .WithName("5")
                  .WithAddress("Saint-Petersburg")
                  .Build();
              Shop shop3 = new Shop.ShopBuilder()
                  .WithName("Dixy")
                  .WithAddress("Omsk")
                  .Build();
              Product product1 = new Product.ProductBuilder()
                  .WithName("Молоко")
                  .Build();
              Product product2 = new Product.ProductBuilder()
                  .WithName("Молоко")
                  .Build();
              Product product3 = new Product.ProductBuilder()
                  .WithName("Молоко")
                  .Build();
              
              _shopService.AddShop(shop1);
              _shopService.AddShop(shop2);
              _shopService.AddShop(shop3);
              _shopService.RegisterProductAtShop(product1, shop1);
              _shopService.RegisterProductAtShop(product2, shop2);
              _shopService.RegisterProductAtShop(product3, shop3);
              
              
              _shopService.DeliveryToShop(shop1, product1, 10);
              _shopService.SetProductPrice(shop1, product1, 100);
              
              _shopService.DeliveryToShop(shop2, product2, 10);
              _shopService.SetProductPrice(shop2, product2, 90);
              
              _shopService.DeliveryToShop(shop3, product3, 10);
              _shopService.SetProductPrice(shop3, product3,40);

              Product neededProduct = new Product.ProductBuilder()
                  .WithName("Молоко")
                  .Build();

              Shop goodShop = _shopService.BestPossibleBuy(neededProduct);
              Assert.True(goodShop.Name == "Dixy");
          }

          [Test]
          public void BuyProductsFromShop_ProductsBoughtCustomerHasUpdatedMoneyCount()
          {
              Shop shop = new Shop.ShopBuilder()
                  .WithName("Ozon")
                  .WithAddress("Moscow City")
                  .Build();
              Product product1 = new Product.ProductBuilder()
                  .WithName("Молоко")
                  .Build();
              Product product2 = new Product.ProductBuilder()
                  .WithName("Сыр")
                  .Build();
              Customer customer = new Customer.CustomerBuilder()
                  .WithName("Alexey")
                  .WithMoney(1000)
                  .Build();

              _shopService.AddShop(shop);
              _shopService.RegisterProductAtShop(product1, shop);
              _shopService.RegisterProductAtShop(product2, shop);
              _shopService.DeliveryToShop(shop, product1, 10);
              _shopService.SetProductPrice(shop, product1, 100);
              _shopService.DeliveryToShop(shop, product2, 5);
              _shopService.SetProductPrice(shop, product2, 120);

              customer = _shopService.Buy(customer, shop, product1, 5);
              customer = _shopService.Buy(customer, shop, product2, 2);
              foreach (Product current in shop.Products)
              {
                  if (product1.Name == current.Name)
                      Assert.True(current.Quantity == 5);

                  if (product2.Name == current.Name)
                      Assert.True(current.Quantity == 3);
                  Assert.True(customer.Money == 260);
              }
          }
         [Test]
         public void BuyMultipleProductsFromShop_ProductsBoughtCustomerHasChangedMoney()
         {
             Shop shop = new Shop.ShopBuilder()
                 .WithName("Ozon")
                 .WithAddress("Moscow City")
                 .Build();
             Product product1 = new Product.ProductBuilder()
                 .WithName("Молоко")
                 .Build();
             Product product2 = new Product.ProductBuilder()
                 .WithName("Сыр")
                 .Build();
             Customer customer = new Customer.CustomerBuilder()
                 .WithName("Alexey")
                 .WithMoney(1000)
                 .Build();

             _shopService.AddShop(shop);
             _shopService.RegisterProductAtShop(product1, shop);
             _shopService.RegisterProductAtShop(product2, shop);
             _shopService.DeliveryToShop(shop, product1, 10);
             _shopService.SetProductPrice(shop, product1, 100);
             _shopService.DeliveryToShop(shop, product2, 5);
             _shopService.SetProductPrice(shop, product2, 120);

             Dictionary<Product, uint> productsDictionary = new ()
             {
                 {product1, 5},
                 {product2, 2}
             };

             customer = _shopService.MultipleBuy(customer, shop, productsDictionary);
             foreach (Product current in shop.Products)
             {
                 if (product1.Name == current.Name)
                     Assert.True(current.Quantity == 5);

                 if (product2.Name == current.Name)
                     Assert.True(current.Quantity == 3);
             }
             Assert.True(customer.Money == 260);
         }

         // [Test]
         // public void AddProdAAddProdB()
         // {
         //     Shop shop = new Shop.ShopBuilder()
         //         .WithName("Ozon")
         //         .WithAddress("Moscow City")
         //         .Build();
         //     Product product1 = new Product.ProductBuilder()
         //         .WithName("Молоко")
         //         .Build();
         //     Product product2 = new Product.ProductBuilder()
         //         .WithName("Сыр")
         //         .Build();
         //     Customer customer = new Customer.CustomerBuilder()
         //         .WithName("Alexey")
         //         .WithMoney(1000)
         //         .Build();
         //     _shopService.AddShop(shop);
         //     _shopService.RegisterProductAtShop(product1, shop);
         //     _shopService.DeliveryToShop(shop, product1, 10);
         //     _shopService.SetProductPrice(shop, product1, 100);
         //     Dictionary<Product, uint> productsDictionary = new ()
         //     {
         //         {product1, 5},
         //         {product2, 2}
         //     };
         //     _shopService.MultipleBuy(customer, shop, productsDictionary);
         //     Assert.True(customer.Money == 500);
         // }
     }
 }