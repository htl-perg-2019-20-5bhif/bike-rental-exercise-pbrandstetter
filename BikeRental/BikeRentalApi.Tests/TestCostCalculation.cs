using BikeRentalApi.Controllers;
using BikeRentalService.Model;
using System;
using Xunit;

namespace BikeRentalApi.Tests
{
    public class TestCostCalculation
    {
        [Fact]
        public void FreeRental()
        {
            DateTime end = DateTime.Now;
            DateTime begin = end.AddMinutes(-15);
            Bike bike = new Bike
            {
                PriceFirstHour = 1.01,
                PricePerAdditionalHour = 3
            };
            Rental rental = new Rental
            {
                Bike = bike,
                RentalBegin = begin,
                RentalEnd = end
            };

            CostCalculator costCalculator = new CostCalculator();
            rental.TotalCost = costCalculator.CalculateTotalCosts(rental);
            Assert.Equal(0, rental.TotalCost);
        }

        [Fact]
        public void OneHourRental()
        {
            DateTime end = DateTime.Now;
            DateTime begin = end.AddMinutes(-59);
            Bike bike = new Bike
            {
                PriceFirstHour = 1.01,
                PricePerAdditionalHour = 3
            };
            Rental rental = new Rental
            {
                Bike = bike,
                RentalBegin = begin,
                RentalEnd = end
            };

            CostCalculator costCalculator = new CostCalculator();
            rental.TotalCost = costCalculator.CalculateTotalCosts(rental);
            Assert.Equal(1.01, rental.TotalCost);
        }

        [Fact]
        public void ThreeHourRental()
        {
            DateTime end = DateTime.Now;
            DateTime begin = end.AddHours(-3);
            Bike bike = new Bike
            {
                PriceFirstHour = 1.01,
                PricePerAdditionalHour = 3
            };
            Rental rental = new Rental
            {
                Bike = bike,
                RentalBegin = begin,
                RentalEnd = end
            };

            CostCalculator costCalculator = new CostCalculator();
            rental.TotalCost = costCalculator.CalculateTotalCosts(rental);
            Assert.Equal(7.01, rental.TotalCost);
        }
    }
}
