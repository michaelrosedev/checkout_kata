using System.Collections.Generic;
using System.Linq;

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
            _scanned.Add(sku);
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
    }
}