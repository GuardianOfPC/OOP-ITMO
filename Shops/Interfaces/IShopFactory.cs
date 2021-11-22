namespace Shops.Interfaces
{
    public interface IShopFactory
    {
        IShopsRepository CreateShopRepository();
        IShopService CreateShopService();
    }
}