﻿using System.Collections.Generic;

namespace Checkout
{
    /// <summary>
    /// Represents a collection of <see cref="Product"/> ready for checkout
    /// </summary>
    public interface IBasket
    {
        /// <summary>
        /// Add a <see cref="Product"/> to the basket
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product);
        
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
    }
}