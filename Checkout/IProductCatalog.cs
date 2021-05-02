namespace Checkout
{
    public interface IProductCatalog
    {
        public Product GetProduct(string sku);
    }
}