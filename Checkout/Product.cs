namespace Checkout
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The stock-keeping unit of the product
        /// </summary>
        public string Sku { get; set; }
        
        /// <summary>
        /// The unit price of the product
        /// </summary>
        public int UnitPrice { get; set; }
    }
}