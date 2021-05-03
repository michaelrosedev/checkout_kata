namespace Checkout.Interfaces
{
    /// <summary>
    /// Represents a virtual checkout
    /// </summary>
    public interface ICheckout
    {
        /// <summary>
        /// Scan a SKU and add to the current checkout/basket
        /// </summary>
        /// <param name="sku"></param>
        void Scan(string sku);
        
        /// <summary>
        /// Calculate the total current price
        /// </summary>
        /// <returns>A <see cref="int"/> representing the current price/value of the checkout/basket</returns>
        int CalculatePrice();
    }
}