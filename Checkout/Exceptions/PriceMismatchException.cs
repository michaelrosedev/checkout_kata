using System;

namespace Checkout.Exceptions
{
    /// <summary>
    /// Custom exception throw when two items with the same SKU have different unit prices
    /// </summary>
    public class PriceMismatchException : Exception
    {
        /// <summary>
        /// Initialize a new instance of <see cref="PriceMismatchException"/>
        /// </summary>
        /// <param name="firstProduct">The first existing <see cref="Product"/></param>
        /// <param name="secondProduct">The new <see cref="Product"/> to be added</param>
        public PriceMismatchException(Product firstProduct, Product secondProduct)
            : base($"The unit price '{secondProduct.UnitPrice}' for sku '{secondProduct.UnitPrice}' does not match the existing unit price of '{firstProduct.UnitPrice}'")
        {
        }
    }
}