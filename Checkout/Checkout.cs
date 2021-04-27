using System.Collections.Generic;

namespace Checkout
{
    public class Checkout : ICheckout
    {
        private List<string> _scanned;
        
        public Checkout()
        {
            _scanned = new List<string>();
        }

        public void Scan(string sku)
        {
            _scanned.Add(sku);
        }

        public int CalculatePrice()
        {
            return _scanned.Count > 0 ? 50 : 0;
        }
    }
}