using NUnit.Framework;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        private Checkout _checkout;
        
        [SetUp]
        public void Setup()
        {
            _checkout = new Checkout();
        }

        [Test]
        public void When_NothingIsScanned_Then_PriceIsZero()
        {
            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(0, totalPrice);
        }
    }
}