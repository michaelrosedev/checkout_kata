using System;

namespace Checkout.Models
{
    /// <summary>
    /// Holds settings for a carrier bag provider
    /// </summary>
    public class CarrierProviderSettings
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CarrierProviderSettings"/>
        /// </summary>
        /// <param name="country">The country. Required.</param>
        /// <param name="unitPrice">The unit price. Must be zero or greater.</param>
        /// <param name="maxItemsPerBag">The maximum number of items per bag. Must be greater than zero.</param>
        /// <exception cref="ArgumentNullException">Thrown when country is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown is unitPrice or maxItemsPerBag are invalid.</exception>
        public CarrierProviderSettings(string country, int unitPrice, int maxItemsPerBag)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentNullException(nameof(country));
            }

            if (unitPrice < 0)
            {
                throw new ArgumentException($"The property '{nameof(unitPrice)}' must be zero or greater");
            }

            if (maxItemsPerBag <= 0)
            {
                throw new ArgumentException($"The property '{nameof(maxItemsPerBag)}' must be greater than zero");
            }
            
            Country = country;
            UnitPrice = unitPrice;
            MaxItemsPerBag = maxItemsPerBag;
        }
        
        public string Country { get; }
        public int UnitPrice { get; }
        
        public int MaxItemsPerBag { get; }
    }
}