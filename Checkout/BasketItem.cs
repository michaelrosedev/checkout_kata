using System;

namespace Checkout
{
    /// <summary>
    /// Represents a collection of <see cref="Product"/> ready for checkout
    /// </summary>
    public class BasketItem
    {
        /// <summary>
        /// Initialize a new instance of <see cref="BasketItem"/>
        /// </summary>
        /// <param name="product">The <see cref="Product"/> that the item corresponds to</param>
        /// <param name="qty">The quantity of the item. Defaults to 1 if not provided.</param>
        public BasketItem(Product product, int qty = 1)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));

            if (qty <= 0)
            {
                throw new ArgumentException($"The property '{nameof(qty)}' must be a positive number");
            }
            
            Qty = qty;
        }

        /// <summary>
        /// Increment the quantity of items of this <see cref="Product"/>
        /// </summary>
        /// <param name="increment">The number to increment by. Defaults to 1 if not provided.</param>
        public void IncrementQty(int increment = 1)
        {
            if (increment <= 0)
            {
                throw new ArgumentException($"The property '{nameof(increment)}' must be a positive number");
            }
            Qty += increment;
        }
        
        /// <summary>
        /// The <see cref="Product"/>
        /// </summary>
        public Product Product { get; }
        
        /// <summary>
        /// The quantity of the associated <see cref="Product"/>
        /// </summary>
        public int Qty { get; private set; }

        /// <summary>
        /// Return the total value of the current product collection
        /// </summary>
        public int TotalValue => Product.UnitPrice * Qty;
    }
}