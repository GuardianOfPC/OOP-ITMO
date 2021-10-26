using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Controllers
{
    public class ShopService : IShopService
    {
        private readonly List<Shop> _shops = new ();
        public IReadOnlyList<Shop> Shops => _shops;

        public Shop AddShop(string name, string address)
        {
            Shop shop = new Shop.ShopBuilder()
                .WithName(name)
                .WithAddress(address)
                .Build();
            _shops.Add(shop);
            shop.ToBuilder().Build();
            return shop;
        }

        public Product RegisterProductAtShop(string name, Shop shop)
        {
            Product product = new Product.ProductBuilder()
                .WithName(name)
                .Build();
            var productsList = (List<Product>)shop.Products;
            productsList.Add(product);
            _shops.Remove(shop);
            Shop outShop = shop
                .ToBuilder()
                .WithProducts(productsList)
                .Build();
            _shops.Add(outShop);
            return product;
        }

        public Shop DeliveryToShop(Shop shop, string name, int quantity)
        {
            var productsList = (List<Product>)shop.Products;
            foreach (Product productCur in productsList)
            {
                if (productCur.Name == name)
                {
                    productsList.Remove(productCur);
                    Product newProduct = productCur
                        .ToBuilder()
                        .WithQuantity(quantity)
                        .Build();
                    productsList.Add(newProduct);
                    shop.ToBuilder()
                        .WithProducts(productsList)
                        .Build();
                    return shop;
                }
            }

            throw new ShopException(
                "There is no such product in this shop");
        }

        public Shop SetProductPrice(Shop shop, string name, int price)
        {
            var productsList = (List<Product>)shop.Products;
            foreach (Product productCur in productsList)
            {
                if (productCur.Name == name)
                {
                    productsList.Remove(productCur);
                    Product newProduct = productCur
                        .ToBuilder()
                        .WithPrice(price)
                        .Build();
                    productsList.Add(newProduct);
                    shop.ToBuilder().WithProducts(productsList).Build();
                    return shop;
                }
            }

            throw new ShopException(
                "There is no such product in this shop");
        }

        public Shop GoodBuy(string name, int quantity)
        {
            int lowestPrice = 100000;
            foreach (Shop currentShop in _shops)
            {
                foreach (Product currentShopProduct in currentShop.Products)
                {
                    if (currentShopProduct.Name == name &&
                        currentShopProduct.Price < lowestPrice)
                    {
                        lowestPrice = currentShopProduct.Price;
                    }
                }
            }

            foreach (Shop currentShop in _shops)
            {
                foreach (Product currentShopProduct in currentShop.Products)
                {
                    if (currentShopProduct.Price == lowestPrice &&
                        currentShopProduct.Quantity >= quantity &&
                        currentShopProduct.Name == name)
                    {
                        return currentShop;
                    }

                    if (currentShopProduct.Price == lowestPrice &&
                        currentShopProduct.Name == name &&
                        currentShopProduct.Quantity < quantity)
                    {
                        throw new ShopException(
                            "Insufficient product");
                    }
                }
            }

            throw new ShopException("No such product");
        }

        public Customer Buy(Customer customer, Shop shop, string name, int quantity)
        {
            var productList = (List<Product>)shop.Products;
            foreach (Product currProduct in productList)
            {
                if (currProduct.Name == name)
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
                    shop.ToBuilder()
                        .WithProducts(productList)
                        .Build();
                    int finalMoney = customer.Money - (currProduct.Price * quantity);
                    customer = customer.ToBuild()
                        .WithMoney(finalMoney)
                        .Build();
                    return customer;
                }
            }

            throw new ShopException("No such product");
        }
    }
}
