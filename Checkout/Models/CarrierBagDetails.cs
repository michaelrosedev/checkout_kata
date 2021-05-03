using System;

namespace Checkout.Models
{
    /// <summary>
    /// Provides details of carrier bags that are required
    /// </summary>
    public class CarrierBagDetails
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CarrierBagDetails"/>
        /// </summary>
        /// <param name="qty">The quantity of bags required</param>
        /// <param name="totalPrice">The total price of the bags required</param>
        /// <exception cref="ArgumentException">Thrown if qty or totalPrice are invalid</exception>
        public CarrierBagDetails(int qty, int totalPrice)
        {
            if (qty < 0)
            {
                throw new ArgumentException($"The property '{nameof(qty)}' must be zero or greater");
            }

            if (totalPrice < 0)
            {
                throw new ArgumentException($"The property '{nameof(qty)}' must be zero or greater");
            }
            
            Qty = qty;
            TotalPrice = totalPrice;
        }
        
        /// <summary>
        /// The quantity of bags required
        /// </summary>
        public int Qty { get; }
        
        /// <summary>
        /// The total price of bags required
        /// </summary>
        public int TotalPrice { get; }
    }
}