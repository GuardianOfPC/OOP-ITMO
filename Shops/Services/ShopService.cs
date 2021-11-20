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
            Product productCur = productsList.FirstOrDefault(productNeeded => productNeeded.Name == product.Name);
            if (productCur == null) throw new ShopException("No such product");
            productsList.Remove(productCur);
            Product newProduct = productCur
                .ToBuilder()
                .WithQuantity(quantity)
                .Build();
            productsList.Add(newProduct);
            ShopsRepository.UpdateShopProducts(shop, productsList);
            return shop;
        }

        public Shop SetProductPrice(Shop shop, Product product, uint price)
        {
            var productsList = (List<Product>)shop.Products;
            Product productCur = productsList.FirstOrDefault(productNeeded => productNeeded.Name == product.Name);
            if (productCur == null) throw new ShopException("No such product");
            productsList.Remove(productCur);
            Product newProduct = productCur
                .ToBuilder()
                .WithPrice(price)
                .Build();
            productsList.Add(newProduct);
            ShopsRepository.UpdateShopProducts(shop, productsList);
            return shop;
        }

        public Shop BestPossibleBuy(Dictionary<Product, uint> productsDictionary)
        {
            Shop resultShop = null;
            bool isProductInShop = false;
            foreach ((Product product, uint quantity) in productsDictionary)
            {
                foreach (Product currentProduct in
                    from currentShop in ShopsRepository.Shops
                    from currentProduct in currentShop.Products
                    where currentProduct.Name == product.Name
                    select currentProduct)
                {
                    isProductInShop = true;
                }

                if (isProductInShop == false) throw new ShopException("No such product");

                var listOfPrices =
                    (from currentShopProduct in ShopsRepository.Shops.SelectMany(currentShop => currentShop.Products)
                        where currentShopProduct.Name == product.Name && currentShopProduct.Quantity >= quantity
                        select currentShopProduct.Price).ToList();

                uint lowestPrice = listOfPrices.Min();
                foreach (Shop currentShop in
                    from currentShop in ShopsRepository.Shops
                    from currentShopProduct in currentShop.Products
                    where currentShopProduct.Name == product.Name && currentShopProduct.Price == lowestPrice
                    select currentShop)
                {
                    resultShop = currentShop;
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