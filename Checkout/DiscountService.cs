﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    public class DiscountService : IDiscountService
    {
        private readonly List<Discount> _discounts;

        public DiscountService(List<Discount> discounts)
        {
            _discounts = discounts;
        }

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