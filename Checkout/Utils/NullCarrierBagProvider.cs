using Checkout.Interfaces;
using Checkout.Models;

namespace Checkout.Utils
{
    /// <summary>
    /// Simple implementation of <see cref="ICarrierBagProvider"/> for when carrier bags do not apply
    /// </summary>
    public class NullCarrierBagProvider : ICarrierBagProvider
    {
        public CarrierBagDetails CalculateCarrierBags(IBasket basket)
        {
            return new()
            {
                Qty = 0,
                TotalPrice = 0
            };
        }
    }
}