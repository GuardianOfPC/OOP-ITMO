using Shops.Interfaces;
using Shops.Models;

namespace Shops.Services
{
    public class ShopsFactory : IShopFactory
    {
        private IShopsRepository _shopsRepository;
        private IShopService _shopService;

        public IShopsRepository CreateShopRepository()
        {
            return _shopsRepository ??= new ShopsRepository();
        }

        public IShopService CreateShopService()
        {
            return _shopService ??= new ShopService(CreateShopRepository());
        }
    }
}