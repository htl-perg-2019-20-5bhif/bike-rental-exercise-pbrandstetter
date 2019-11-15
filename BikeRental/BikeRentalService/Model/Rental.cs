using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    class Rental
    {
        /// <summary>
        /// Unique number identifies a rentre
        /// </summary>
        public int RentalId { get; set; }

        /// <summary>
        /// Mandatory reference to the customer who rented the bike
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        /// <summary>
        /// Mandatory reference to the rented bike
        /// </summary>
        [Required]
        public int BikeId { get; set; }

        public Bike Bike { get; set; }

        /// <summary>
        /// Mandatory date and time indicates when the bike rental begins
        /// </summary>
        [Required]
        public DateTime RentalBegin { get; set; }

        /// <summary>
        /// Indicates the date and time when the bike rental ends,
        /// Must be greater than <see cref="RentalBegin"/>
        /// </summary>
        public DateTime RentalEnd { get; set; }

        /// <summary>
        /// Total cost in Euro for renting a bike, two decimal places,
        /// Minimum value is 0.00
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// boolean flag indicating whether the rental has already been paid by the customer,
        /// can only be true if the rental has already ended (i.e. <see cref="RentalEnd"/> and total <see cref="TotalCost"/> are set)
        /// </summary>
        public bool Paid { get; set; }
    }
}
