using System;
using Checkout.Models;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class CarrierProviderSettingsTests
    {
        [Test]
        public void When_CreatingNewProviderSettings_Then_TheCountryIsStoredCorrectly()
        {
            const string country = "Scotland";

            var settings = new CarrierProviderSettings(country, 10, 10);
            
            Assert.AreEqual(country, settings.Country);
        }

        [Test]
        public void When_CreatingNewProviderSettings_Then_TheUnitPriceIsStoredCorrectly()
        {
            const int unitPrice = 22;

            var settings = new CarrierProviderSettings("Germany", unitPrice, 1);
            
            Assert.AreEqual(unitPrice, settings.UnitPrice);
        }

        [Test]
        public void When_CreatingNewProviderSettings_Then_TheMaxItemsValueIsStoredCorrectly()
        {
            const int maxItemsPerBag = 20;

            var settings = new CarrierProviderSettings("Belgium", 1, maxItemsPerBag);
            
            Assert.AreEqual(maxItemsPerBag, settings.MaxItemsPerBag);
        }

        [Test]
        public void When_CreatingNewProviderSettings_Then_CountryCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new CarrierProviderSettings(null, 10, 10);
            });
        }

        [Test]
        public void When_CreatingNewProviderSettings_Then_CountryCannotBeAnEmptyString()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new CarrierProviderSettings(string.Empty, 10, 10);
            });
        }

        [TestCase(-10)]
        public void When_CreatingNewProviderSettings_Then_UnitPriceMustBeZeroOrGreater(int unitPrice)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new CarrierProviderSettings("France", unitPrice, 10);
            });
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void When_CreatingNewProviderSettings_Then_MaxItemsPerBagMustBeGreaterThanZero(int maxItemsPerBag)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new CarrierProviderSettings("Ireland", 10, maxItemsPerBag);
            });
        }
    }
}