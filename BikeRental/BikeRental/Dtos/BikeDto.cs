using BikeRentalService.Model;
using System;
using System.Collections.Generic;

namespace BikeRentalApi.Dtos
{
    public class BikeDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public long PurchaseDate { get; set; }
        public string Notes { get; set; }
        public long LastService { get; set; }
        public double PriceFirstHour { get; set; }
        public double PricePerAdditionalHour { get; set; }
        public string BikeCategory { get; set; }

        public static List<BikeDto> ToBikeDto(List<Bike> allBikes)
        {
            var bikes = new List<BikeDto>();
            foreach (Bike b in allBikes)
            {
                bikes.Add(ToBikeDto(b));

            }
            return bikes;
        }

        public static BikeDto ToBikeDto(Bike b)
        {
            return new BikeDto()
            {
                Id = b.Id,
                BikeCategory = b.BikeCategory,
                Brand = b.Brand,
                Notes = b.Notes,
                PriceFirstHour = b.PriceFirstHour,
                PricePerAdditionalHour = b.PricePerAdditionalHour,
                PurchaseDate = new DateTimeOffset(b.PurchaseDate).ToUnixTimeMilliseconds(),
                LastService = new DateTimeOffset(b.LastService).ToUnixTimeMilliseconds()
            };
        }

        public static List<Bike> FromBikeDto(List<BikeDto> allBikeDtos)
        {
            var bikes = new List<Bike>();
            foreach (BikeDto b in allBikeDtos)
            {
                bikes.Add(FromBikeDto(b));

            }
            return bikes;
        }

        public static Bike FromBikeDto(BikeDto b)
        {
            return new Bike()
            {
                Id = b.Id,
                BikeCategory = b.BikeCategory,
                Brand = b.Brand,
                Notes = b.Notes,
                PriceFirstHour = b.PriceFirstHour,
                PricePerAdditionalHour = b.PricePerAdditionalHour,
                PurchaseDate = new DateTime(b.PurchaseDate),
                LastService = new DateTime(b.LastService)
            };
        }
    }
}
