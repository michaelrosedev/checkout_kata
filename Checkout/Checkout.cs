using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly IEnumerable<Product> _products;
        private readonly IDiscountService _discountService;
        private List<Product> _scanned;
        
        public Checkout(
            IEnumerable<Product> products,
            IDiscountService discountService)
        {
            _products = products;
            _discountService = discountService;
            _scanned = new List<Product>();
        }

        public void Scan(string sku)
        {
            var product = _products.FirstOrDefault(p => p.Sku == sku);
            if (product == null)
            {
                throw new UnrecognisedProductException();
            }
            
            _scanned.Add(product);
        }

        public int CalculatePrice()
        {
            if (_scanned.Count == 0)
            {
                return 0;
            }
            
            ApplyDiscounts();
            
            var runningTotal = 0;

            foreach (var scan in _scanned)
            {
                runningTotal += scan.UnitPrice;
            }

            return runningTotal;
        }

        private void ApplyDiscounts()
        {
            var discounts = _discountService.GetDiscounts(_scanned);
            if (discounts.Any())
            {
                _scanned.AddRange(discounts);
            }
        }
    }
}