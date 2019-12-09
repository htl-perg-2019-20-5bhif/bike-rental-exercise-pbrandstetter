using BikeRentalService.Model;
using System;
using System.Collections.Generic;

namespace BikeRentalApi.Dtos
{
    public class RentalDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BikeId { get; set; }
        public long RentalBegin { get; set; }
        public long RentalEnd { get; set; }
        public double TotalCost { get; set; }
        public bool Paid { get; set; }

        public static List<RentalDto> ToRentalDto(List<Rental> allRentals)
        {
            var rentals = new List<RentalDto>();
            foreach (Rental r in allRentals)
            {
                rentals.Add(ToRentalDto(r));
            }
            return rentals;
        }

        public static RentalDto ToRentalDto(Rental r)
        {
            return new RentalDto()
            {
                Id = r.Id,
                BikeId = r.BikeId,
                CustomerId = r.CustomerId,
                Paid = r.Paid,
                TotalCost = r.TotalCost,
                RentalBegin = new DateTimeOffset(r.RentalBegin).ToUnixTimeMilliseconds(),
                RentalEnd = new DateTimeOffset(r.RentalEnd).ToUnixTimeMilliseconds()
            };
        }

        public static List<Rental> FromRentalDto(List<RentalDto> allrRentalDtos)
        {
            var rentals = new List<Rental>();
            foreach (RentalDto r in allrRentalDtos)
            {
                rentals.Add(FromRentalDto(r));
            }
            return rentals;
        }

        public static Rental FromRentalDto(RentalDto r)
        {
            return new Rental()
            {
                Id = r.Id,
                BikeId = r.BikeId,
                CustomerId = r.CustomerId,
                Paid = r.Paid,
                TotalCost = r.TotalCost,
                RentalBegin = new DateTime(r.RentalBegin),
                RentalEnd = new DateTime(r.RentalEnd),
            };
        }
    }
}
