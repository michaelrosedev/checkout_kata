using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountService _discountService;
        
        [SetUp]
        public void Setup()
        {
            _discountService = new DiscountService(new List<Discount>(0));
        }

        [Test]
        public void When_NoDiscountIsAvailable_Then_NoDiscountIsReturned()
        {
            var basket = new List<Product>(0);
            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.AreEqual(0, discounts.Count);
        }

        [Test]
        public void When_ASingleDiscountIsAvailable_Then_ASingleDiscountIsReturned()
        {
            var discountsAvailable = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                }
            };
            _discountService = new DiscountService(discountsAvailable);

            var basket = new List<Product>
            {
                new()
                {
                    Sku = "A",
                    UnitPrice = 50
                },
                new()
                {
                    Sku = "A",
                    UnitPrice = 50
                },
                new()
                {
                    Sku = "A",
                    UnitPrice = 50
                }
            };

            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.NotNull(firstDiscount);
            
            Assert.AreEqual(-20, firstDiscount.UnitPrice);
        }
    }
}