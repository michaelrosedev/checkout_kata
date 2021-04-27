using System.Collections.Generic;
using System.Linq;
using Checkout.Exceptions;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private readonly IEnumerable<Product> _products;
        private List<string> _scanned;
        
        public Checkout(IEnumerable<Product> products)
        {
            _products = products;
            _scanned = new List<string>();
        }

        public void Scan(string sku)
        {
            if (Exists(sku))
            {
                _scanned.Add(sku);
            }
            else
            {
                throw new UnrecognisedProductException();
            }
        }

        public int CalculatePrice()
        {
            if (_scanned.Count == 0)
            {
                return 0;
            }
            
            var runningTotal = 0;

            foreach (var scan in _scanned)
            {
                var product = _products.FirstOrDefault(p => p.Sku == scan);
                runningTotal += product.UnitPrice;
            }

            return runningTotal;
        }

        private bool Exists(string sku)
        {
            return _products.Any(x => x.Sku == sku);
        }
    }
}