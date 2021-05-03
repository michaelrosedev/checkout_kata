using System;

namespace Checkout
{
    /// <summary>
    /// A special <see cref="IProduct"/> implementation for discounts
    /// </summary>
    public class DiscountedProduct : IProduct
    {
        private const string DiscountPrefix = "disc_";
        
        /// <summary>
        /// Initialize a new <see cref="DiscountedProduct"/>
        /// </summary>
        /// <param name="sku">The SKU of the related <see cref="IProduct"/></param>
        /// <param name="unitPrice">The unit price, in this case the discount.</param>
        /// <exception cref="ArgumentNullException">Thrown if a SKU is not provided</exception>
        /// <exception cref="ArgumentException">Throw if the UnitPrice is zero or below</exception>
        public DiscountedProduct(string sku, int unitPrice)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }

            if (unitPrice >= 0)
            {
                throw new ArgumentException($"The {nameof(unitPrice)} value must be less than zero (0)");
            }
            
            Sku = GenerateSku(sku);
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// The unique SKU of the discount
        /// </summary>
        public string Sku { get; }
        
        /// <summary>
        /// The unit price of the discount
        /// </summary>
        public int UnitPrice { get; }

        private static string GenerateSku(string sku)
        {
            return $"{DiscountPrefix}{sku}";
        }
    }
}