using System.Collections.Generic;
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
            foreach (Product productCur in productsList)
            {
                if (productCur.Name != product.Name) continue;
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
            foreach (Product productCur in productsList)
            {
                if (productCur.Name != product.Name) continue;
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

        public Shop BestPossibleBuy(Product product)
        {
            uint? lowestPrice = null;
            foreach (Shop currentShop in ShopsRepository.Shops)
            {
                foreach (Product currentShopProduct in currentShop.Products)
                {
                    if (currentShopProduct.Name == product.Name) lowestPrice = currentShopProduct.Price;
                }
            }

            if (lowestPrice == null) throw new ShopException("No such product");

            foreach (Shop currentShop in ShopsRepository.Shops)
            {
                foreach (Product currentShopProduct in currentShop.Products)
                {
                    if (currentShopProduct.Name == product.Name &&
                        currentShopProduct.Price < lowestPrice)
                    {
                        lowestPrice = currentShopProduct.Price;
                    }
                }
            }

            foreach (Shop currentShop in ShopsRepository.Shops)
            {
                foreach (Product currentShopProduct in currentShop.Products)
                {
                    if (currentShopProduct.Quantity < product.Quantity &&
                        currentShopProduct.Price == lowestPrice &&
                        currentShopProduct.Name == product.Name)
                    {
                        throw new
                            ShopException("Insufficient product");
                    }

                    if (currentShopProduct.Quantity >= product.Quantity &&
                        currentShopProduct.Price == lowestPrice &&
                        currentShopProduct.Name == product.Name)
                    {
                        return currentShop;
                    }
                }
            }

            throw new ShopException("No such product");
        }

        public Customer Buy(Customer customer, Shop shop, Product product, uint quantity)
        {
            var productList = (List<Product>)shop.Products;
            foreach (Product currProduct in productList)
            {
                if (currProduct.Name == product.Name)
                {
                    if (currProduct.Quantity < quantity)
                    {
                        throw new ShopException("Insufficient products");
                    }

                    if (customer.Money < (currProduct.Price * quantity))
                    {
                        throw new ShopException("Not enough money");
                    }

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
