namespace Checkout.Interfaces
{
    /// <summary>
    /// Generic product interface
    /// </summary>
    public interface IProduct
    {
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