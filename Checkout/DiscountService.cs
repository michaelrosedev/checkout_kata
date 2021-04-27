using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    /// <summary>
    /// A service for determining which discount(s) apply to a given collection of <see cref="Product"/> 
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly List<Discount> _discounts;

        /// <summary>
        /// Initialize a new instance of <see cref="DiscountService"/>
        /// </summary>
        /// <param name="discounts">The current applicable collection of discount(s)</param>
        public DiscountService(List<Discount> discounts)
        {
            _discounts = discounts;
        }

        /// <summary>
        /// Get the list of discounts that apply to the provided list of <see cref="Product"/>
        /// </summary>
        /// <param name="basket">The current basket contents</param>
        /// <returns>A list of <see cref="Product"/> where a negative UnitPrice represents the calculated discount</returns>
        public List<Product> GetDiscounts(List<Product> basket)
        {
            var discounts = new List<Product>();

            var groupedBasket = basket.GroupBy(b => b.Sku);

            foreach (var group in groupedBasket)
            {
                var qty = group.Count();
                var discount = _discounts.FirstOrDefault(d => d.Sku == group.Key);
                var currentProduct = basket.First(b => b.Sku == group.Key);
                if (discount == null)
                {
                    continue;
                }

                if (qty >= discount.TriggerQuantity)
                {
                    var discountQty = Math.Floor((double) qty / discount.TriggerQuantity);

                    var shouldCapDiscount = ShouldCapDiscount(discount, currentProduct.UnitPrice);
                    
                    for (var i = 0; i < discountQty; i++)
                    {
                        if (shouldCapDiscount)
                        {
                            discounts.Add(new Product
                            {
                                Sku = discount.Sku,
                                UnitPrice = discount.TriggerQuantity * -currentProduct.UnitPrice
                            });
                        }
                        else
                        {
                            discounts.Add(new Product
                            {
                                Sku = discount.Sku,
                                UnitPrice = discount.DiscountValue
                            });
                        }
                    }
                }
            }
            
            return discounts;
        }

        private static bool ShouldCapDiscount(Discount discount, int unitPrice)
        {
            return -discount.DiscountValue > unitPrice;
        }
    }
}