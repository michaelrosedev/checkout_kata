using Checkout.Models;

namespace Checkout.Interfaces
{
    /// <summary>
    /// Interface for determining how many carrier bags are required
    /// </summary>
    public interface ICarrierBagProvider
    {
        /// <summary>
        /// Calculate the carrier bags required based on the <see cref="IBasket"/> contents
        /// </summary>
        /// <param name="basket">The current <see cref="IBasket"/></param>
        /// <returns>A <see cref="CarrierBagDetails"/></returns>
        CarrierBagDetails CalculateCarrierBags(IBasket basket);
    }
}