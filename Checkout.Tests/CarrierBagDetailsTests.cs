using System;
using Checkout.Models;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class CarrierBagDetailsTests
    {
        [TestCase(-1)]
        [TestCase(-10)]
        public void When_CreatingNewInstance_Then_QtyMustBeZeroOrGreater(int qty)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new CarrierBagDetails(qty, 10);
            });
        }

        [TestCase(-1)]
        [TestCase(-10)]
        public void When_CreatingNewInstance_Then_TotalPriceMustBeZeroOrGreater(int totalPrice)
        {
            Assert.Throws <ArgumentException>(() =>
            {
                _ = new CarrierBagDetails(10, totalPrice);
            });
        }
    }
}