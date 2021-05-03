using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;

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
        public void AddProduct(Product product)
        {
            var basketItem = _contents.FirstOrDefault(c => c.Product.Sku == product.Sku);
            if (basketItem == null)
            {
                _contents.Add(new BasketItem(product));
            }
            else
            {
                EnsureMatchingUnitPrice(basketItem.Product, product);
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

        private static void EnsureMatchingUnitPrice(Product firstProduct, Product secondProduct)
        {
            if (firstProduct.UnitPrice != secondProduct.UnitPrice)
            {
                throw new PriceMismatchException(firstProduct, secondProduct);
            }
        }
    }
}