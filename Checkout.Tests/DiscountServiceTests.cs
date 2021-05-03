using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountService _discountService;
        private Mock<IDiscountRepository> _discountRepositoryMock;
        private List<Discount> _discounts;
        private Mock<IBasket> _basketMock;
        
        [SetUp]
        public void Setup()
        {
            _discounts = new List<Discount>(0);
            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _discountRepositoryMock.Setup(dr => dr.GetDiscountForSku(It.IsAny<string>()))
                .Returns<string>((sku) => _discounts.FirstOrDefault(d => d.Sku == sku));
            
            _discountService = new DiscountService(_discountRepositoryMock.Object);

            _basketMock = new Mock<IBasket>();
        }

        [Test]
        public void When_NoItemsInBasket_Then_NoDiscountIsReturned()
        {
            var basket = new Basket();
            var discounts = _discountService.GetDiscounts(basket);
            
            Assert.AreEqual(0, discounts.Count);
        }

        [Test]
        public void When_NoDiscountIsAvailable_Then_NoDiscountIsReturned()
        {
            _discounts = new List<Discount>
            {
                new()
                {
                    Sku = "XYZ",
                    TriggerQuantity = 6,
                    DiscountValue = -12
                }
            };
            
            var basketContents = new List<BasketItem>
            {
                new(new Product("A", 50), 12)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);

            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(0, discounts.Count);

        }

        [Test]
        public void When_ASingleDiscountIsAvailable_Then_ASingleDiscountIsReturned()
        {
            _discounts = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                }
            };

            var basketContents = new List<BasketItem>
            {
                new(new Product("A", 50), 3)
            };

            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.NotNull(firstDiscount);
            
            Assert.AreEqual(-20, firstDiscount.UnitPrice);
        }

        [Test]
        public void When_MultipleDiscountsAreAvailable_Then_MultipleDiscountsAreReturned()
        {
            _discounts = new List<Discount>
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

            var productA = new Product("A", 50);
            var productB = new Product("B", 30);

            var basketContents = new List<BasketItem>
            {
                new(productA, 3),
                new(productB, 2)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
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
            _discounts = new List<Discount>
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

            var basketContents = new List<BasketItem>
            {
                new(new Product(sku, unitPrice), qty)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.AreEqual(expectedDiscount, firstDiscount?.UnitPrice);
        }

        [TestCase("A", 50, 6, -40)]
        [TestCase("A", 50, 9, -60)]
        [TestCase("B", 30, 4, -30)]
        [TestCase("B", 30, 6, -45)]
        public void When_ASingleDiscountAppliesMoreThanOnce_Then_MultipleDiscountsAreReturned(
            string sku,
            int unitPrice,
            int qty,
            int expectedDiscount)
        {
            _discounts = new List<Discount>
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
            
            var basketContents = new List<BasketItem>
            {
                new(new Product(sku, unitPrice), qty)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.That(discounts.Count > 1, "Expected more than one discount to be returned");

            var totalDiscount = discounts.Sum(d => d.UnitPrice);
            
            Assert.AreEqual(expectedDiscount, totalDiscount);
        }

        [TestCase("A", 5, 3, 3, -20)]
        [TestCase("B", 10, 2, 2, -15)]
        public void When_DiscountIsGreaterThanUnitPrice_Then_DiscountIsCappedAt100Percent(
            string sku,
            int unitPrice,
            int triggerThreshold,
            int qty,
            int defaultDiscountRate)
        {
            _discounts = new List<Discount>
            {
                new()
                {
                    Sku = sku,
                    TriggerQuantity = triggerThreshold,
                    DiscountValue = defaultDiscountRate
                }
            };
            
            var basketContents = new List<BasketItem>
            {
                new(new Product(sku, unitPrice), qty)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.AreEqual(-unitPrice * qty, firstDiscount?.UnitPrice);
        }

        [Test]
        public void When_ProductDiscountExists_Then_SkuIsExtended()
        {
            _discounts = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                }
            };

            var basketContents = new List<BasketItem>
            {
                new(new Product("A", 50), 3)
            };

            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);
            
            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(1, discounts.Count);

            var firstDiscount = discounts.FirstOrDefault();
            
            Assert.NotNull(firstDiscount);

            var firstBasketItem = basketContents.FirstOrDefault();
            
            Assert.NotNull(firstBasketItem);
            
            Assert.AreNotEqual(firstBasketItem.Product.Sku, firstDiscount.Sku);
            
        }

        [Test]
        public void When_TriggerThresholdIsNotReached_ThenDiscountDoesNotApply()
        {
            _discounts = new List<Discount>
            {
                new()
                {
                    Sku = "A",
                    TriggerQuantity = 3,
                    DiscountValue = -20
                }
            };
            
            var basketContents = new List<BasketItem>
            {
                new(new Product("A", 50), 2)
            };
            
            _basketMock.Setup(x => x.GetContents())
                .Returns(basketContents);

            var discounts = _discountService.GetDiscounts(_basketMock.Object);
            
            Assert.AreEqual(0, discounts.Count);
        }
    }
}