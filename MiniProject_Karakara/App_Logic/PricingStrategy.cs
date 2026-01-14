using System;

namespace MiniProject_Karakara
{
    // Design Pattern: Strategy (simplified)
    // Encapsulates the pricing/tax calculation logic.
    // We could have different strategies (e.g. HolidayPricing, VIPPricing).
    public class PricingStrategy
    {
        private const decimal TAX_RATE = 0.06m;

        public decimal CalculateTax(decimal subTotal)
        {
            return subTotal * TAX_RATE;
        }

        public decimal CalculateTotal(decimal subTotal)
        {
            return subTotal + CalculateTax(subTotal);
        }
    }
}
