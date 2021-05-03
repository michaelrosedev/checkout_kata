using Checkout.Interfaces;
using Checkout.Models;
using Moq;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class CarrierBagProviderUkTests
    {
        private CarrierBagProvider _carrierBagProvider;
        private Mock<IBasket> _basketMock;
        
        [SetUp]
        public void Setup()
        {
            _carrierBagProvider = new CarrierBagProvider();
            _basketMock = new Mock<IBasket>();
        }
        
        [Test]
        public void When_NoItemsScanned_Then_NoCarrierChargeApplies()
        {
            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(0);
            
            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(0, carrierBagDetails.Qty);
            Assert.AreEqual(0, carrierBagDetails.TotalPrice);
        }

        [Test]
        public void When_ASingleItemIsScanned_Then_ASingleCarrierChargeApplies()
        {
            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(1);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(1, carrierBagDetails.Qty);
            Assert.AreEqual(5, carrierBagDetails.TotalPrice);
        }

        [TestCase(5, 1)]
        [TestCase(10, 2)]
        [TestCase(7, 2)]
        [TestCase(12, 3)]
        public void When_MultipleItemsAreScanned_Then_TheCorrectNumberOfBagsIsApplied(
            int itemQty,
            int expectedNumberOfBags)
        {
            _basketMock.Setup(b => b.TotalItemQuantity()).Returns(itemQty);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(expectedNumberOfBags, carrierBagDetails.Qty);
        }

        [TestCase(5, 5)]
        [TestCase(7, 10)]
        [TestCase(10, 10)]
        [TestCase(12, 15)]
        [TestCase(20, 20)]
        public void When_MultipleItemsAreScanned_Then_TheCorrectPriceIsApplied(
            int itemQty,
            int expectedPrice)
        {
            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(itemQty);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(expectedPrice, carrierBagDetails.TotalPrice);
        }

        [Test]
        public void When_CarrierBagProviderIsOperatingInWales_And_NoItemsAdded_Then_NoChargeIsApplied()
        {
            var carrierProviderSettings = new CarrierProviderSettings("Wales", 10, 5);
            
            _carrierBagProvider = new CarrierBagProvider(carrierProviderSettings);

            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(0);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(0, carrierBagDetails.Qty);
            Assert.AreEqual(0, carrierBagDetails.TotalPrice);
        }

        [Test]
        public void When_CarrierBagProviderIsOperatingInWales_And_ASingleItemIsAdded_Then_ASingleCarrierChargeApplies()
        {
            var carrierProviderSettings = new CarrierProviderSettings("Wales", 10, 5);
            
            _carrierBagProvider = new CarrierBagProvider(carrierProviderSettings);

            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(1);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(1, carrierBagDetails.Qty);
            Assert.AreEqual(10, carrierBagDetails.TotalPrice);
        }
        
        [TestCase(5, 1)]
        [TestCase(10, 2)]
        [TestCase(7, 2)]
        [TestCase(12, 3)]
        public void When_CarrierBagProviderIsOperatingInWales_And_MultipleItemsAreScanned_Then_TheCorrectNumberOfBagsIsApplied(
            int itemQty,
            int expectedNumberOfBags)
        {
            var carrierProviderSettings = new CarrierProviderSettings("Wales", 10, 5);
            
            _carrierBagProvider = new CarrierBagProvider(carrierProviderSettings);
            
            _basketMock.Setup(b => b.TotalItemQuantity()).Returns(itemQty);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(expectedNumberOfBags, carrierBagDetails.Qty);
        }
        
        [TestCase(5, 10)]
        [TestCase(7, 20)]
        [TestCase(10, 20)]
        [TestCase(12, 30)]
        [TestCase(20, 40)]
        public void When_CarrierBagProviderIsOperatingInWales_And_MultipleItemsAreScanned_Then_TheCorrectPriceIsApplied(
            int itemQty,
            int expectedPrice)
        {
            var carrierProviderSettings = new CarrierProviderSettings("Wales", 10, 5);
            
            _carrierBagProvider = new CarrierBagProvider(carrierProviderSettings);
            
            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(itemQty);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(expectedPrice, carrierBagDetails.TotalPrice);
        }

        [TestCase(3, 1)]
        [TestCase(5, 2)]
        [TestCase(6, 2)]
        [TestCase(9, 3)]
        [TestCase(10, 4)]
        public void When_BagCanHoldThreeItemsMax_AndMultipleItemsAreProvided_Then_TheCorrectNumberOfBagsIsReturned(
            int itemQty,
            int expectedNumberOfBags)
        {
            var carrierProviderSettings = new CarrierProviderSettings("Spain", 20, 3);

            _carrierBagProvider = new CarrierBagProvider(carrierProviderSettings);

            _basketMock.Setup(b => b.TotalItemQuantity())
                .Returns(itemQty);

            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basketMock.Object);
            
            Assert.AreEqual(expectedNumberOfBags, carrierBagDetails.Qty);
        }
    }
}