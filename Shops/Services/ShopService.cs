using System.Collections.Generic;
using System.Linq;
using Shops.Interfaces;
using Shops.Models;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopService : IShopService
    {
        public ShopService(IShopsRepository shopsRepository) => ShopsRepository = shopsRepository;

        public IShopsRepository ShopsRepository { get; }

        public Shop AddShop(Shop shop)
        {
            ShopsRepository.Add(shop);
            return shop;
        }

        public Product RegisterProductAtShop(Product product, Shop shop)
        {
            var productsList = (List<Product>)shop.Products;
            productsList.Add(product);
            ShopsRepository.Remove(shop);
            Shop outShop = shop
                .ToBuilder()
                .WithProducts(productsList)
                .Build();
            ShopsRepository.Add(outShop);
            return product;
        }

        public Shop DeliveryToShop(Shop shop, Product product, uint quantity)
        {
            var productsList = (List<Product>)shop.Products;
            foreach (Product productCur in productsList.Where(productCur => productCur.Name == product.Name))
            {
                productsList.Remove(productCur);
                Product newProduct = productCur
                    .ToBuilder()
                    .WithQuantity(quantity)
                    .Build();
                productsList.Add(newProduct);
                ShopsRepository.UpdateShopProducts(shop, productsList);
                return shop;
            }

            throw new ShopException(
                "There is no such product in this shop");
        }

        public Shop SetProductPrice(Shop shop, Product product, uint price)
        {
            var productsList = (List<Product>)shop.Products;
            foreach (Product productCur in productsList.Where(productCur => productCur.Name == product.Name))
            {
                productsList.Remove(productCur);
                Product newProduct = productCur
                    .ToBuilder()
                    .WithPrice(price)
                    .Build();
                productsList.Add(newProduct);
                ShopsRepository.UpdateShopProducts(shop, productsList);
                return shop;
            }

            throw new ShopException(
                "There is no such product in this shop");
        }

        public Shop BestPossibleBuy(Dictionary<Product, uint> productsDictionary)
        {
            Shop resultShop = null;
            uint lowestPrice = ShopsRepository.Shops.First().Products[0].Price;
            bool isProductInShop = false;
            foreach ((Product product, uint quantity) in productsDictionary)
            {
                foreach (var currentShopProduct in ShopsRepository.Shops.SelectMany(currentShop => currentShop.Products))
                {
                    if (currentShopProduct.Name == product.Name) isProductInShop = true;
                    if (currentShopProduct.Name == product.Name
                        && currentShopProduct.Price < lowestPrice)
                    {
                        lowestPrice = currentShopProduct.Price;
                    }
                }

                if (isProductInShop == false) throw new ShopException("No such product");

                foreach (Shop currentShop in ShopsRepository.Shops)
                {
                    foreach (Product currentShopProduct in currentShop.Products)
                    {
                        if (currentShopProduct.Name == product.Name
                            && currentShopProduct.Price == lowestPrice)
                        {
                            if (currentShopProduct.Quantity < quantity) throw new ShopException("Insufficient Product");
                            resultShop = currentShop;
                        }
                    }
                }
            }

            if (resultShop == null) throw new ShopException("No such Shop");

            return resultShop;
        }

        public Customer Buy(Customer customer, Shop shop, Product product, uint quantity)
        {
            var productList = (List<Product>)shop.Products;
            foreach (Product currProduct in productList.Where(currProduct => currProduct.Name == product.Name))
            {
                if (currProduct.Quantity < quantity) throw new ShopException("Insufficient products");
                if (customer.Money < (currProduct.Price * quantity)) throw new ShopException("Not enough money");
                productList.Remove(currProduct);
                Product newProd = currProduct
                    .ToBuilder()
                    .WithQuantity(currProduct.Quantity - quantity).Build();
                productList.Add(newProd);
                ShopsRepository.UpdateShopProducts(shop, productList);
                uint finalMoney = customer.Money - (currProduct.Price * quantity);
                customer = customer.ToBuild()
                    .WithMoney(finalMoney)
                    .Build();
                return customer;
            }

            throw new ShopException("No such product");
        }

        public Customer MultipleBuy(Customer customer, Shop shop, Dictionary<Product, uint> productsDictionary)
        {
            foreach ((Product product, uint quantity) in productsDictionary)
            {
                customer = Buy(customer, shop, product, quantity);
            }

            return customer;
        }
    }
}