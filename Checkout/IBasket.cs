using System.Collections.Generic;

namespace Checkout
{
    public interface IBasket
    {
        public void AddProduct(Product product);
        public IEnumerable<BasketItem> GetContents();
        public int TotalItemQuantity();
    }
}