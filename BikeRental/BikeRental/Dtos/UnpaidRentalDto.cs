using System;

namespace BikeRentalApi.Dtos
{
    public class UnpaidRentalDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RentalId { get; set; }
        public DateTime RentalBegin { get; set; }
        public DateTime RentalEnd { get; set; }
        public double TotalCost { get; set; }
    }
}
