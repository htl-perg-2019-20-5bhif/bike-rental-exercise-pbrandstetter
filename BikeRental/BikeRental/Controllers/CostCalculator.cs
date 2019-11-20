using BikeRentalService.Model;
using System;

namespace BikeRentalApi.Controllers
{
    public class CostCalculator
    {
        /// <summary>
        /// Calculates the costs of a rental based on the bikes prices and the duration
        /// </summary>
        /// <param name="rental">Rental of which the costs should be calculated</param>
        /// <returns>Total costs of a rental</returns>
        public double CalculateTotalCosts(Rental rental)
        {
            var duration = rental.RentalEnd - rental.RentalBegin;
            double totalCost = 0;
            if (duration.TotalMinutes <= 15)
            {
                return totalCost;
            }
            totalCost += rental.Bike.PriceFirstHour;
            var additionalHours = duration.TotalHours - 1;
            if (additionalHours > 0)
            {
                totalCost += (int)(Math.Ceiling(additionalHours)) * rental.Bike.PricePerAdditionalHour;
            }
            return totalCost;
        }
    }
}
