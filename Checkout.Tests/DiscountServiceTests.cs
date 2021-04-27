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

        [Test]
        public void When_MultipleDiscountsAreAvailable_Then_MultipleDiscountsAreReturned()
        {
            var discountsAvailable = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                },
                new()
                {
                    Sku = "B",
                    TriggerQuantity = 2,
                    DiscountValue = -15
                },
            };
            _discountService = new DiscountService(discountsAvailable);

            var productA = new Product
            {
                Sku = "A",
                UnitPrice = 50
            };
            var productB = new Product
            {
                Sku = "B",
                UnitPrice = 30
            };

            var basket = new List<Product>
            {
                productA,
                productA,
                productA,
                productB,
                productB
            };

            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.AreEqual(2, discounts.Count);

            var firstDiscount = discounts[0];
            Assert.AreEqual(-20, firstDiscount.UnitPrice);

            var secondDiscount = discounts[1];
            Assert.AreEqual(-15, secondDiscount.UnitPrice);
        }

        [TestCase("A", 50, 4, -20)]
        [TestCase("A", 50, 5, -20)]
        [TestCase("B", 30, 3, -15)]
        public void When_ASingleProductExceedsTheDiscountThreshold_Then_TheCorrectDiscountIsReturned(
            string sku,
            int unitPrice,
            int qty,
            int expectedDiscount)
        {
            var discountsAvailable = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                },
                new()
                {
                    Sku = "B",
                    TriggerQuantity = 2,
                    DiscountValue = -15
                }
            };
            
            _discountService = new DiscountService(discountsAvailable);

            var basket = new List<Product>();
            
            for (var i = 0; i < qty; i++)
            {
                basket.Add(new Product
                {
                    Sku = sku,
                    UnitPrice = unitPrice
                });                
            }

            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.AreEqual(expectedDiscount, firstDiscount?.UnitPrice);
        }

        [TestCase("A", 50, 6, -40)]
        public void When_ASingleDiscountAppliesMoreThanOnce_Then_MultipleDiscountsAreReturned(
            string sku,
            int unitPrice,
            int qty,
            int expectedDiscount)
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

            var basket = new List<Product>();

            for (var i = 0; i < qty; i++)
            {
                basket.Add(new Product
                {
                    Sku = sku,
                    UnitPrice = unitPrice
                });
            }

            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.That(discounts.Count > 1, "Expected more than one discount to be returned");

            var totalDiscount = discounts.Sum(d => d.UnitPrice);
            
            Assert.AreEqual(expectedDiscount, totalDiscount);
        }
    }
}