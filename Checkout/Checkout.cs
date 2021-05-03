using System;
using System.Linq;
using Checkout.Exceptions;
using Checkout.Interfaces;
using Checkout.Models;

namespace Checkout
{
    /// <inheritdoc />
    public class Checkout : ICheckout
    {
        private readonly IProductCatalog _productCatalog;
        private readonly IDiscountService _discountService;
        private readonly IBasket _basket;
        private readonly ICarrierBagProvider _carrierBagProvider;
        
        /// <summary>
        /// Initialize a new instance of <see cref="Checkout"/>
        /// </summary>
        /// <param name="productCatalog">The current product catalog</param>
        /// <param name="discountService">A service for processing discounts, if available</param>
        /// <param name="basket">The basket for adding items to</param>
        /// <param name="carrierBagProvider">A service for determining if carrier bags apply</param>
        public Checkout(
            IProductCatalog productCatalog,
            IDiscountService discountService,
            IBasket basket,
            ICarrierBagProvider carrierBagProvider)
        {
            _productCatalog = productCatalog ?? throw new ArgumentNullException(nameof(productCatalog));
            _discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
            _basket = basket ?? throw new ArgumentNullException(nameof(basket));
            _carrierBagProvider = carrierBagProvider ?? throw new ArgumentNullException(nameof(carrierBagProvider));
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
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentNullException(nameof(sku));
            }
            
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
            
            AddCarrierBags();
            ApplyDiscounts();

            return _basket.TotalValue();
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

        private void AddCarrierBags()
        {
            var carrierBagDetails = _carrierBagProvider.CalculateCarrierBags(_basket);
            if (carrierBagDetails == null || carrierBagDetails.Qty == 0)
            {
                return;
            }

            var product = new Product($"carriers_x_{carrierBagDetails.Qty}", carrierBagDetails.TotalPrice);
            _basket.AddProduct(product);
        }
    }
}