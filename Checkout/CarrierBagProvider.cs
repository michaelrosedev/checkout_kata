using System;
using Checkout.Interfaces;
using Checkout.Models;
using Checkout.Utils;

namespace Checkout
{
    /// <inheritdoc />
    public class CarrierBagProvider : ICarrierBagProvider
    {
        private readonly CarrierProviderSettings _settings;

        /// <summary>
        /// Initialize a new instance with a default <see cref="CarrierProviderSettings"/>
        /// </summary>
        public CarrierBagProvider()
        {
            _settings = Defaults.DefaultCarrierProviderSettings;
        }

        /// <summary>
        /// Initialize a new instance with a specific <see cref="CarrierProviderSettings"/>
        /// </summary>
        /// <param name="settings">The <see cref="CarrierProviderSettings"/> to apply</param>
        public CarrierBagProvider(CarrierProviderSettings settings)
        {
            _settings = settings;
        }
        
        /// <inheritdoc />
        public CarrierBagDetails CalculateCarrierBags(IBasket basket)
        {
            var totalItemQty = basket.TotalItemQuantity();
            
            if (totalItemQty == 0)
            {
                return new CarrierBagDetails
                {
                    Qty = 0,
                    TotalPrice = 0
                };
            }
            
            var requiredBagCount = (int)Math.Ceiling((double)totalItemQty / _settings.MaxItemsPerBag);
            
            return new CarrierBagDetails
            {
                Qty = requiredBagCount,
                TotalPrice = _settings.UnitPrice * requiredBagCount
            };
        }
    }
}