﻿using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly IEnumerable<Product> _products;
        private readonly IDiscountService _discountService;
        private readonly List<Product> _basket;
        
        /// <summary>
        /// Initialize a new instance of <see cref="Checkout"/>
        /// </summary>
        /// <param name="products">The current list of available products</param>
        /// <param name="discountService">A service for processing discounts, if available</param>
        public Checkout(
            IEnumerable<Product> products,
            IDiscountService discountService)
        {
            _products = products;
            _discountService = discountService;
            _basket = new List<Product>();
        }

        /// <summary>
        /// Scan a product by SKU
        /// </summary>
        /// <param name="sku">The stock-keeping unit to scan</param>
        /// <exception cref="UnrecognisedProductException">
        /// Thrown if the SKU is not recognised
        /// </exception>
        public void Scan(string sku)
        {
            var product = _products.FirstOrDefault(p => p.Sku == sku);
            if (product == null)
            {
                throw new UnrecognisedProductException();
            }
            
            _basket.Add(product);
        }

        /// <summary>
        /// Calculate the current price of the basket
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public int CalculatePrice()
        {
            if (_basket.Count == 0)
            {
                return 0;
            }
            
            ApplyDiscounts();
            
            var runningTotal = 0;

            foreach (var scan in _basket)
            {
                runningTotal += scan.UnitPrice;
            }

            return runningTotal;
        }

        private void ApplyDiscounts()
        {
            var discounts = _discountService.GetDiscounts(_basket);
            if (discounts.Any())
            {
                _basket.AddRange(discounts);
            }
        }
    }
}