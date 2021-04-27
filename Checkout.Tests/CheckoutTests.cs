using System.Collections.Generic;
using NUnit.Framework;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        private Checkout _checkout;
        
        [SetUp]
        public void Setup()
        {
            var products = new List<Product>
            {
                new()
                {
                    Sku = "A",
                    UnitPrice = 50
                },
                new()
                {
                    Sku = "B",
                    UnitPrice = 30
                },
                new()
                {
                    Sku = "C",
                    UnitPrice = 20
                },
                new()
                {
                    Sku = "D",
                    UnitPrice = 15
                }
            };
            _checkout = new Checkout(products);
        }

        [Test]
        public void When_NothingIsScanned_Then_PriceIsZero()
        {
            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(0, totalPrice);
        }

        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C",20 )]
        [TestCase("D", 15)]
        public void When_ItemIsScanned_Then_TotalEqualsItemValue(string sku, int expectedPrice)
        {
            _checkout.Scan(sku);
            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedPrice, totalPrice);
        }

        [TestCase("A", 2, 100)]
        public void When_MultipleOfSameItemScanned_WithoutTriggeringDiscount_Then_TotalIsCalculatedCorrectly(
            string sku,
            int qty,
            int expectedPrice)
        {
            for (var i = 0; i < qty; i++)
            {
                _checkout.Scan(sku);
            }

            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedPrice, totalPrice);
        }
    }
}