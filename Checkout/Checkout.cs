using System.Linq;
using Checkout.Exceptions;
using Checkout.Interfaces;

namespace Checkout
{
    /// <inheritdoc />
    public class Checkout : ICheckout
    {
        private readonly IProductCatalog _productCatalog;
        private readonly IDiscountService _discountService;
        private readonly IBasket _basket;
        
        /// <summary>
        /// Initialize a new instance of <see cref="Checkout"/>
        /// </summary>
        /// <param name="productCatalog">The current product catalog</param>
        /// <param name="discountService">A service for processing discounts, if available</param>
        /// <param name="basket">The basket for adding items to</param>
        public Checkout(
            IProductCatalog productCatalog,
            IDiscountService discountService,
            IBasket basket)
        {
            _productCatalog = productCatalog;
            _discountService = discountService;
            _basket = basket;
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
            var product = _productCatalog.GetProduct(sku);
            if (product == null)
            {
                throw new UnrecognisedProductException();
            }
            
            _basket.AddProduct(product);
        }

        /// <summary>
        /// Calculate the current price of the basket
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public int CalculatePrice()
        {
            if (_basket.TotalItemQuantity() == 0)
            {
                return 0;
            }
            
            ApplyDiscounts();

            return _basket.GetContents().Sum(basketItem => basketItem.TotalValue);
        }

        private void ApplyDiscounts()
        {
            var discounts = _discountService.GetDiscounts(_basket);
            
            if (!discounts.Any())
            {
                return;
            }
            
            foreach (var discount in discounts)
            {
                _basket.AddProduct(discount);
            }
        }
    }
}