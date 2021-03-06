using System.Collections.Generic;
using Checkout.Models;

namespace Checkout.Interfaces
{
    /// <summary>
    /// Represents a collection of <see cref="IProduct"/> ready for checkout
    /// </summary>
    public interface IBasket
    {
        /// <summary>
        /// Add a <see cref="IProduct"/> to the basket
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(IProduct product);
        
        /// <summary>
        /// Get the contents of the basket
        /// </summary>
        /// <returns>A <see cref="List{BasketItem}"/></returns>
        public IEnumerable<BasketItem> GetContents();
        
        /// <summary>
        /// Get the count of the total quantity of items
        /// </summary>
        /// <returns>The total number of items in the <see cref="Basket"/></returns>
        public int TotalItemQuantity();

        /// <summary>
        /// Get the total value of the items in the basket
        /// </summary>
        /// <returns>The sum of the value of the items in the basket</returns>
        public int TotalValue();
    }
}