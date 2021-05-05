using System;
using Checkout.Models;
using NUnit.Framework;

namespace Checkout.Tests
{
    [TestFixture]
    public class BasketItemTests
    {
        [Test]
        public void When_CreatingANewBasketItem_ThenTheQuantityIsOneByDefault()
        {
            var product = new Product("Apples", 10);
            
            var basketItem = new BasketItem(product);
            
            Assert.AreEqual(1, basketItem.Qty);
        }

        [Test]
        public void When_CreatingANewBasketItem_WithQuantity_ThenTheCorrectQuantityIsRecorded()
        {
            var product = new Product("Cheese", 5);

            const int qty = 4;
            
            var basketItem = new BasketItem(product, qty);
            
            Assert.AreEqual(qty, basketItem.Qty);
        }

        [Test]
        public void When_CreatingANewBasketItem_Then_ProductIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = new BasketItem(null);
            });
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void When_CreatingANewBasketItem_AndQuantityIsProvided_Then_QuantityMustBeAPositiveNumber(int qty)
        {
            var product = new Product("Coat", 20);

            Assert.Throws<ArgumentException>(() =>
            {
                _ = new BasketItem(product, qty);
            });
        }

        [TestCase(1, 1, 2)]
        [TestCase(10, 10, 20)]
        [TestCase(1, 20, 21)]
        public void When_IncrementingTheQuantityOfABasketItem_Then_TheCorrectValueIsCalculated(
            int startingQty,
            int increment,
            int expectedQty)
        {
            var product = new Product("Bananas", 2);

            var basketItem = new BasketItem(product, startingQty);

            basketItem.IncrementQty(increment);
            
            Assert.AreEqual(expectedQty, basketItem.Qty);
        }

        [Test]
        public void When_IncrementingTheQuantityOfABasketItem_Then_TheIncrementDefaultsTo1IfNotProvided()
        {
            var product = new Product("Fish", 2);

            var basketItem = new BasketItem(product);
            
            basketItem.IncrementQty();
            
            Assert.AreEqual(2, basketItem.Qty);
        }

        [TestCase(0)]
        [TestCase(-20)]
        public void When_IncrementingTheQuantityOfABasketItem_Then_TheIncrementMustBeAPositiveNumber(int increment)
        {
            var basketItem = new BasketItem(new Product("Coffee", 12));
            
            Assert.Throws<ArgumentException>(() =>
            {
                basketItem.IncrementQty(increment);
            });
        }

        [Test]
        public void When_AddingAnItemToTheBasket_Then_TheBasketItemProvidesTheTotalValueOfTheBasketItem()
        {
            var basketItem = new BasketItem(new Product("Tea", 10));

            var totalValue = basketItem.TotalValue;
            
            Assert.AreEqual(10, totalValue);
        }

        [TestCase(10, 10, 100)]
        [TestCase(12, 5, 60)]
        public void When_AddingMultipleQuantities_Then_TheBasketItemProvidesTheTotalValueOfTheBasketItem(
            int unitPrice,
            int qty,
            int expectedTotalValue)
        {
            var basketItem = new BasketItem(
                new Product("Potatoes", unitPrice),
                qty);
            
            Assert.AreEqual(expectedTotalValue, basketItem.TotalValue);
        }
    }
}