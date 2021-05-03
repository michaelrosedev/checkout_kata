using System;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void When_CreatingAProduct_Then_TheSkuIsStoredCorrectly()
        {
            const string sku = "Carrots";

            var product = new Product(sku, 1);
            
            Assert.AreEqual(sku, product.Sku);
        }

        [Test]
        public void When_CreatingAProduct_And_TheSkuIsNull_ThenAnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Product(null, 1);
            });
        }

        [Test]
        public void When_CreatingAProduct_And_TheSkuIsEmpty_ThenAnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new Product(string.Empty, 1);
            });
        }

        [TestCase(0)]
        [TestCase(-20)]
        public void When_CreatingAProduct_Then_TheUnitPriceMustBeGreaterThanZero(
            int unitPrice)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new Product("SKU", unitPrice);
            });
        }
    }
}