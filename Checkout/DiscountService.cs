using System;
using System.Collections.Generic;

namespace Checkout
{
    /// <summary>
    /// A service for determining which discount(s) apply to a given collection of <see cref="Product"/> 
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        /// <summary>
        /// Initialize a new instance of <see cref="DiscountService"/>
        /// </summary>
        /// <param name="discountRepository">Access to the current available discounts</param>
        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        /// <summary>
        /// Get the list of discounts that apply to the provided list of <see cref="Product"/>
        /// </summary>
        /// <param name="basket">The current <see cref="IBasket"/></param>
        /// <returns>A list of <see cref="Product"/> where a negative UnitPrice represents the calculated discount</returns>
        public List<Product> GetDiscounts(IBasket basket)
        {
            var discounts = new List<Product>();

            foreach (var basketItem in basket.GetContents())
            {
                var discount = _discountRepository.GetDiscountForSku(basketItem.Product.Sku);
                if (discount == null)
                {
                    continue;
                }
                
                var currentProduct = basketItem.Product;

                if (basketItem.Qty < discount.TriggerQuantity)
                {
                    continue;
                }
                
                var discountQty = Math.Floor((double) basketItem.Qty / discount.TriggerQuantity);
                var shouldCapDiscount = ShouldCapDiscount(discount, currentProduct.UnitPrice);

                for (var i = 0; i < discountQty; i++)
                {
                    discounts.Add(
                        shouldCapDiscount
                            ? CreateDiscountForSku(discount.Sku, discount.TriggerQuantity * -currentProduct.UnitPrice)
                            : CreateDiscountForSku(discount.Sku, discount.DiscountValue)
                    );
                }
            }
            
            return discounts;
        }

        private static bool ShouldCapDiscount(Discount discount, int unitPrice)
        {
            return -discount.DiscountValue > unitPrice;
        }

        private static Product CreateDiscountForSku(string sku, int discount)
        {
            return new()
            {
                Sku = $"{sku}_discount",
                UnitPrice = discount
            };
        }
    }
}