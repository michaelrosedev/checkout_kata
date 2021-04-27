namespace Checkout
{
    public interface ICheckout
    {
        void Scan(string sku);
        int CalculatePrice();
    }
}