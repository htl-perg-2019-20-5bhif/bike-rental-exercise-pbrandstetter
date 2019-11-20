using BikeRentalService.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalApi.Model.Validators
{
    public class PaidValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (Rental)validationContext.ObjectInstance;
            if (rental.Paid && rental.TotalCost == 0)
            {
                return new ValidationResult("Free rentals cannot be paid");
            }
            if (rental.RentalEnd == DateTime.MinValue && rental.Paid)
            {
                return new ValidationResult("Paid can only be set to true when rental has ended");
            }
            return ValidationResult.Success;
        }
    }
}
