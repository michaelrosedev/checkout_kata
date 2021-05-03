using System;
using Checkout.Interfaces;

namespace Checkout
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public class Product : IProduct
    {
        /// <summary>
        /// Initialize a new instance of <see cref="Product"/>
        /// </summary>
        /// <param name="sku">The stock-keeping unit</param>
        /// <param name="unitPrice">The unit price. Must be greater than zero (0)</param>
        /// <exception cref="ArgumentNullException">Thrown if the sku is null or empty</exception>
        /// <exception cref="ArgumentException">Thrown if an invalid unitPrice is provided</exception>
        public Product(string sku, int unitPrice)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }

            if (unitPrice <= 0)
            {
                throw new ArgumentException($"The '{nameof(unitPrice)}' must be greater than zero (0)");
            }
            
            Sku = sku;
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// The stock-keeping unit of the product
        /// </summary>
        public string Sku { get; }
        
        /// <summary>
        /// The unit price of the product
        /// </summary>
        public int UnitPrice { get; }
    }
}