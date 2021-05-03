using System.Collections.Generic;
using System.Linq;
using Checkout.Extensions;
using Checkout.Interfaces;
using Checkout.Models;

namespace Checkout
{
    /// <inheritdoc />
    public class Basket : IBasket
    {
        private readonly List<BasketItem> _contents;
        
        /// <summary>
        /// Initialize a new instance of <see cref="Basket"/>
        /// </summary>
        public Basket()
        {
            _contents = new List<BasketItem>();
        }

        /// <inheritdoc />
        public void AddProduct(IProduct product)
        {
            var basketItem = _contents.FirstOrDefault(c => c.Product.Sku == product.Sku);
            if (basketItem == null)
            {
                _contents.Add(new BasketItem(product));
            }
            else
            {
                basketItem.Product.EnsureMatchingUnitPrice(product);
                basketItem.IncrementQty();
            }
        }
        
        /// <inheritdoc />
        public IEnumerable<BasketItem> GetContents()
        {
            return _contents;
        }

        /// <inheritdoc />
        public int TotalItemQuantity()
        {
            return _contents.Sum(c => c.Qty);
        }

        /// <inheritdoc />
        public int TotalValue()
        {
            return _contents.Sum(basketItem => basketItem.Qty * basketItem.Product.UnitPrice);
        }
    }
}