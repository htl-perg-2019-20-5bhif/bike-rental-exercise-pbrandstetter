using BikeRentalService.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalApi.Model.Validators
{
    public class RentalEndBeginValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (Rental)validationContext.ObjectInstance;
            if (rental.RentalBegin > rental.RentalEnd)
            {
                return new ValidationResult("Rental end must be after rental start");
            }
            else if (rental.RentalEnd != DateTime.MinValue)
            {
                return new ValidationResult("Rental already has a end time");
            }
            return ValidationResult.Success;
        }
    }
}
