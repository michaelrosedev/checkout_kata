using System;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class DiscountedProductTests
    {
        [Test]
        public void When_CreatingADiscountedProduct_And_SkuIsNull_Then_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new DiscountedProduct(null, -1);
            });
        }

        [Test]
        public void When_CreatingADiscountedProduct_And_SkuIsEmpty_Then_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new DiscountedProduct(string.Empty, -1);
            });
        }

        [TestCase(0)]
        [TestCase(20)]
        public void When_CreatingADiscountedProduct_Then_TheUnitPriceMustBeBelowZero(int unitPrice)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new DiscountedProduct("sku", unitPrice);
            });
        }

        [TestCase("A", "disc_A")]
        [TestCase("IcedFingers", "disc_IcedFingers")]
        public void When_CreatingADiscountFromASku_Then_TheSkuOfTheDiscountCanBeDifferentiated(
            string productSku,
            string expectedDiscountSku)
        {
            var discountedProduct = new DiscountedProduct(productSku, -1);
            
            Assert.AreEqual(expectedDiscountSku, discountedProduct.Sku);
        }
    }
}