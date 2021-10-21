using System.Linq;
using NUnit.Framework;
using Shops.Services;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private ShopService _shopService;

        [SetUp]
        public void Setup()
        {
            _shopService = new ();
        }

        [Test]
        public void AddShopToList()
        {
            Shop shop = _shopService.AddShop("Ozon", "Moscow City");
            Assert.True(_shopService.Shops.Contains(shop));
        }
    }
}