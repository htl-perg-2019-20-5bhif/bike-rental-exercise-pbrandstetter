using BikeRentalService.Model;
using System;
using System.Collections.Generic;

namespace BikeRentalApi.Dtos
{
    public class UnpaidRentalDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RentalId { get; set; }
        public long RentalBegin { get; set; }
        public long RentalEnd { get; set; }
        public double TotalCost { get; set; }

        public static List<UnpaidRentalDto> ToUnpaidRentalDto(List<Rental> allRentals)
        {
            var rentals = new List<UnpaidRentalDto>();

            foreach (Rental r in allRentals)
            {
                rentals.Add(ToUnpaidRentalDto(r));
            }
            return rentals;
        }

        public static UnpaidRentalDto ToUnpaidRentalDto(Rental rental)
        {
            return new UnpaidRentalDto()
            {
                RentalId = rental.Id,
                CustomerId = rental.CustomerId,
                FirstName = rental.Customer.FirstName,
                LastName = rental.Customer.LastName,
                TotalCost = rental.TotalCost,
                RentalBegin = new DateTimeOffset(rental.RentalBegin).ToUnixTimeMilliseconds(),
                RentalEnd = new DateTimeOffset(rental.RentalEnd).ToUnixTimeMilliseconds()
            };
        }
    }
}
