using Checkout.Exceptions;
using Checkout.Interfaces;

namespace Checkout.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IProduct"/>
    /// </summary>
    public static class ProductExtensions
    {
        /// <summary>
        /// Ensure that 2 products have a matching unit price
        /// </summary>
        /// <remarks>
        /// This can be used to guard against dynamic price changes mid-way through a transaction.
        /// At this point, all products must have the same unit price when the SKU matches, otherwise
        /// this exception will be thrown.
        /// </remarks>
        /// <param name="product">The current <see cref="IProduct"/></param>
        /// <param name="otherProduct">The new <see cref="IProduct"/> to compare to</param>
        /// <exception cref="PriceMismatchException">
        /// Thrown if the 2 <see cref="IProduct"/> instances have different unit prices
        /// </exception>
        public static void EnsureMatchingUnitPrice(this IProduct product, IProduct otherProduct)
        {
            if (product.UnitPrice != otherProduct.UnitPrice)
            {
                throw new PriceMismatchException(product, otherProduct);
            }
        }
    }
}