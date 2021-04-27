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
        public void When_ItemIsScanned_Then_TotalEqualsItemValue(string sku, int expectedPrice)
        {
            _checkout.Scan(sku);
            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedPrice, totalPrice);
        }
    }
}