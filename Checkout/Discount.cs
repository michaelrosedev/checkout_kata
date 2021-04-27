namespace Checkout
{
    /// <summary>
    /// An object representing a discount based on SKU
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// The SKU that the discount applies to
        /// </summary>
        public string Sku { get; set; }
        
        /// <summary>
        /// The quantity of the specified SKU at which the discount applies
        /// </summary>
        public int TriggerQuantity { get; set; }
        
        /// <summary>
        /// The value of the discount represented as the value to *remove* from the total price
        /// </summary>
        public int DiscountValue { get; set; }
    }
}