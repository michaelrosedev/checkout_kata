﻿using System.Collections.Generic;

namespace Checkout
{
    /// <summary>
    /// Represents a service for processing/determining discount(s)
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Retrieve a list of <see cref="Discount"/> that apply to the provided basket
        /// </summary>
        /// <param name="basket">The current <see cref="IBasket"/></param>
        /// <returns>A <see cref="List{Product}"/></returns>
        List<Product> GetDiscounts(IBasket basket);
    }
}