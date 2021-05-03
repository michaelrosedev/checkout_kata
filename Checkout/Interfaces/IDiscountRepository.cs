using Checkout.Models;

namespace Checkout.Interfaces
{
    /// <summary>
    /// Represents a repository of discounts
    /// </summary>
    public interface IDiscountRepository
    {
        /// <summary>
        /// Get the <see cref="Discount"/> for a specific SKU
        /// </summary>
        /// <param name="sku">The SKU of the <see cref="Product"/> to retrieve the <see cref="Discount"/> for</param>
        /// <returns>The <see cref="Discount"/></returns>
        Discount GetDiscountForSku(string sku);
    }
}