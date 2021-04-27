using System.Collections.Generic;

namespace Checkout
{
    public interface IDiscountService
    {
        List<Product> GetDiscounts(List<Product> basket);
    }
}