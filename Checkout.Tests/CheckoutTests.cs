using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;
using Checkout.Interfaces;
using Checkout.Models;
using Checkout.Utils;
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
        private List<IProduct> _discounts;
        
        [SetUp]
        public void Setup()
        {
            _products = new List<Product>
            {
                new ("A", 50),
                new("B", 30),
                new("C", 20),
                new("D", 15),
            };
            
            _discounts = new List<IProduct>(0);
            
            _productCatalogMock = new Mock<IProductCatalog>();
            _productCatalogMock.Setup(pc => pc.GetProduct(It.IsAny<string>()))
                .Returns<string>(sku => _products.FirstOrDefault(p => p.Sku == sku));
            
            _discountServiceMock = new Mock<IDiscountService>();
            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<Basket>()))
                .Returns(_discounts);

            var basket = new Basket();
            
            _checkout = new Checkout(_productCatalogMock.Object, _discountServiceMock.Object, basket, new NullCarrierBagProvider());
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
            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<Basket>()))
                .Returns(new List<IProduct>
                {
                    new DiscountedProduct(sku, -20)
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
            _discounts = new List<IProduct>
            {
                new DiscountedProduct(firstSku, firstDiscount),
                new DiscountedProduct(secondSku, secondDiscount)
            };
            
            _products = new List<Product>
            {
                new(firstSku, firstUnitPrice),
                new(secondSku, secondUnitPrice)
            };

            _discountServiceMock.Setup(ds => ds.GetDiscounts(It.IsAny<Basket>()))
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

        [Test]
        public void When_ScanningSku_And_SkuIsNull_Then_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _checkout.Scan(null);
            });
        }

        [Test]
        public void When_ScanningSku_And_SkuIsEmpty_Then_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _checkout.Scan(string.Empty);
            });
        }

        [TestCase("A", 10, 10, 10, 2, 110)]
        [TestCase("A", 12, 5, 5, 1, 65)]
        public void When_CarrierChargesApply_Then_ChargesAreIncludedInTheTotal(
            string sku,
            int unitPrice,
            int qty,
            int totalBagPrice,
            int bagQty,
            int expectedTotal)
        {
            var carrierBagProviderMock = new Mock<ICarrierBagProvider>();
            carrierBagProviderMock.Setup(c => c.CalculateCarrierBags(It.IsAny<IBasket>()))
                .Returns(new CarrierBagDetails
                {
                    TotalPrice = totalBagPrice,
                    Qty = bagQty
                });

            _productCatalogMock.Setup(pc => pc.GetProduct(It.IsAny<string>()))
                .Returns(new Product(sku, unitPrice));
            
            _checkout = new Checkout(
                _productCatalogMock.Object,
                _discountServiceMock.Object,
                new Basket(),
                carrierBagProviderMock.Object);

            for (var i = 0; i < qty; i++)
            {
                _checkout.Scan(sku);    
            }
            
            var totalPrice = _checkout.CalculatePrice();
            
            Assert.AreEqual(expectedTotal, totalPrice);
        }
    }
}