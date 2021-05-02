namespace Checkout
{
    /// <summary>
    /// Represents a product catalog
    /// </summary>
    public interface IProductCatalog
    {
        /// <summary>
        /// Find a <see cref="Product"/> with a specific SKU
        /// </summary>
        /// <param name="sku">The SKU of the <see cref="Product"/> to retrieve</param>
        /// <returns>The identified <see cref="Product"/></returns>
        public Product GetProduct(string sku);
    }
}