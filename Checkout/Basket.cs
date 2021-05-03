using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    public class Basket : IBasket
    {
        private readonly List<BasketItem> _contents;
        
        public Basket()
        {
            _contents = new List<BasketItem>();
        }

        public void AddProduct(Product product)
        {
            var basketItem = _contents.FirstOrDefault(c => c.Product.Sku == product.Sku);
            if (basketItem == null)
            {
                _contents.Add(new BasketItem
                {
                    Product = product,
                    Qty = 1
                });
            }
            else
            {
                basketItem.Qty += 1;
            }
        }
        
        public IEnumerable<BasketItem> GetContents()
        {
            return _contents;
        }

        public int TotalItemQuantity()
        {
            return _contents.Sum(c => c.Qty);
        }
    }
}