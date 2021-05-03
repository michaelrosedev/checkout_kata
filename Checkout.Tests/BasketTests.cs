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

        [Test]
        public void When_CalculatingBasketTotalValueForASingleItem_Then_TotalIsCalculatedCorrectly()
        {
            const int price = 10;
            
            var product = new Product("Z", price);
            
            _basket.AddProduct(product);

            var total = _basket.TotalValue();
            
            Assert.AreEqual(price, total);
        }

        [TestCase(10, 10, 100)]
        [TestCase(5, 24, 120)]
        public void When_CalculatingBasketTotalValueForMultipleOfSameItem_Then_TotalIsCalculatedCorrectly(
            int unitPrice,
            int qty,
            int expectedValue)
        {
            var product = new Product("some_sku", unitPrice);

            for (var i = 0; i < qty; i++)
            {
                _basket.AddProduct(product);
            }

            var totalValue = _basket.TotalValue();
            
            Assert.AreEqual(expectedValue, totalValue);
        }

        [Test]
        public void When_CalculatingBasketTotalValueIncludingDiscounts_Then_TotalValueIsCalculatedCorrectly()
        {
            var product = new Product("pears", 10);
            var discount = new DiscountedProduct("disc_pears", -5);
            
            _basket.AddProduct(product);
            _basket.AddProduct(discount);

            var total = _basket.TotalValue();
            
            Assert.AreEqual(5, total);
        }
    }
}