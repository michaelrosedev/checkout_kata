using Checkout.Exceptions;
using Checkout.Models;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class BasketTests
    {
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            _basket = new Basket();
        }
        
        [Test]
        public void When_NothingAddedToBasket_Then_TotalItemQuantityIsZero()
        {
            var itemCount = _basket.TotalItemQuantity();
            
            Assert.AreEqual(0, itemCount);
        }

        [Test]
        public void When_ItemAddedToBasket_Then_BasketContainsSingleItem()
        {
            var product = new Product("X", 10);

            _basket.AddProduct(product);

            var itemCount = _basket.TotalItemQuantity();
            
            Assert.AreEqual(1, itemCount);
        }

        [Test]
        public void When_MultipleOfSameItemAddedToBasket_Then_BasketContainsCorrectNumberOfItems()
        {
            const int targetItemCount = 10;
            var product = new Product("X", 10);

            for (var i = 0; i < targetItemCount; i++)
            {
                _basket.AddProduct(product);
            }

            var itemCount = _basket.TotalItemQuantity();
            
            Assert.AreEqual(targetItemCount, itemCount);
        }
        
        [Test]
        public void When_MultipleOfSameItemAddedToBasket_Then_BasketContainsAllItems()
        {
            const int targetItemCount = 10;
            var product = new Product("X", 10);

            for (var i = 0; i < targetItemCount; i++)
            {
                _basket.AddProduct(product);
            }

            var contents = _basket.GetContents();

            foreach (var item in contents)
            {
                Assert.AreEqual(product, item.Product);
            }
        }

        [Test]
        public void When_MultipleDifferentItemsAddedToBasket_Then_BasketContainsCorrectNumberOfItems()
        {
            const int targetItemCount = 10;
            var productX = new Product("X", 10);
            var productY = new Product("Y", 10);

            for (var i = 0; i < targetItemCount; i++)
            {
                _basket.AddProduct(productX);
                _basket.AddProduct(productY);
            }

            var itemCount = _basket.TotalItemQuantity();
            
            Assert.AreEqual(2 * targetItemCount, itemCount);
        }

        [Test]
        public void When_SecondProductWithSameSkuButDifferentUnitPriceAddedToBasket_Then_AnExceptionIsThrown()
        {
            var productA = new Product("A", 1);

            var productB = new Product("A", 2);
            
            Assert.AreNotEqual(productA.UnitPrice, productB.UnitPrice);

            _basket.AddProduct(productA);

            Assert.Throws<PriceMismatchException>(() =>
            {
                _basket.AddProduct(productB);
            });
        }
    }
}