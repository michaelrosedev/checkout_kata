using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;
using Moq;
using NUnit.Framework;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        private Checkout _checkout;
        private Mock<IDiscountService> _discountServiceMock;
        private Mock<IProductCatalog> _productCatalogMock;
        private List<Product> _products;
        private List<Product> _discounts;
        
        [SetUp]
        public void Setup()
        {
            _products = new List<Product>
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
            
            _discounts = new List<Product>(0);
            
            _productCatalogMock = new Mock<IProductCatalog>();
            _productCatalogMock.Setup(pc => pc.GetProduct(It.IsAny<string>()))
                .Returns<string>(sku => _products.FirstOrDefault(p => p.Sku == sku));
            
            _discountServiceMock = new Mock<IDiscountService>();
            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<List<Product>>()))
                .Returns(_discounts);
            _checkout = new Checkout(_productCatalogMock.Object, _discountServiceMock.Object);
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
        [TestCase("C", 2, 40)]
        [TestCase("C", 3, 60)]
        [TestCase("D", 2, 30)]
        [TestCase("D", 3, 45)]
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

        [Test]
        public void When_ProductIsUnrecognised_ScanThrowsAnException()
        {
            const string unrecognisedSku = "X";

            Assert.Throws<UnrecognisedProductException>(() =>
            {
                _checkout.Scan(unrecognisedSku);
            });
        }

        [TestCase("a")]
        [TestCase("b")]
        [TestCase("c")]
        [TestCase("d")]
        public void When_ScanningASku_Then_TheCorrectCasingMustBeUsed(string sku)
        {
            Assert.Throws<UnrecognisedProductException>(() =>
            {
                _checkout.Scan(sku);
            });
        }

        [TestCase(2, 1, 3, 2, 220)]
        public void When_MixedContentsAreScanned_AndDiscountsNotTriggered_ThenTotalIsCalculatedCorrectly(
            int qtyA,
            int qtyB,
            int qtyC,
            int qtyD,
            int expectedPrice)
        {
            for (var i = 0; i < qtyA; i++)
            {
                _checkout.Scan("A");
            }

            for (var i = 0; i < qtyB; i++)
            {
                _checkout.Scan("B");
            }

            for (var i = 0; i < qtyC; i++)
            {
                _checkout.Scan("C");
            }

            for (var i = 0; i < qtyD; i++)
            {
                _checkout.Scan("D");
            }

            var total = _checkout.CalculatePrice();
            
            Assert.AreEqual(total, expectedPrice);
        }

        [TestCase("A", 3, 130)]
        public void When_SingleItemDiscountIsTriggered_Then_TheTotalReflectsTheDiscount(
            string sku,
            int qty,
            int expectedPrice)
        {
            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<List<Product>>()))
                .Returns(new List<Product>
                {
                    new()
                    {
                        Sku = sku,
                        UnitPrice = -20
                    }
                });
            
            for (var i = 0; i < qty; i++)
            {
                _checkout.Scan(sku);
            }

            var total = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedPrice, total);
        }

        [TestCase("A", -20, 50, 3, "B", -15, 30, 2)]
        public void When_MultipleDiscountsAreTriggered_Then_MultipleDiscountsAreIncludedInTheTotal(
            string firstSku,
            int firstDiscount,
            int firstUnitPrice,
            int firstQty,
            string secondSku,
            int secondDiscount,
            int secondUnitPrice,
            int secondQty)
        {
            _discounts = new List<Product>
            {
                new()
                {
                    Sku = firstSku,
                    UnitPrice = firstDiscount
                },
                new()
                {
                    Sku = secondSku,
                    UnitPrice = secondDiscount
                }
            };
            
            _products = new List<Product>
            {
                new()
                {
                    Sku = firstSku,
                    UnitPrice = firstUnitPrice
                },
                new()
                {
                    Sku = secondSku,
                    UnitPrice = secondUnitPrice
                },
            };

            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<List<Product>>()))
                .Returns(_discounts);

            _productCatalogMock.Setup(pc => pc.GetProduct(It.IsAny<string>()))
                .Returns<string>(sku => _products.FirstOrDefault(p => p.Sku == sku));
            
            var expectedPrice = (firstQty * firstUnitPrice) + (secondQty * secondUnitPrice) + firstDiscount + secondDiscount;

            for (var i = 0; i < firstQty; i++)
            {
                _checkout.Scan(firstSku);
            }

            for (var i = 0; i < secondQty; i++)
            {
                _checkout.Scan(secondSku);
            }
            
            var total = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedPrice, total);
        }
    }
}