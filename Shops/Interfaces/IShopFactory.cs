namespace Shops.Interfaces
{
    public interface IShopFactory
    {
        public IShopsRepository CreateShopRepository();
        public IShopService CreateShopService();
    }
}