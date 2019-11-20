using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalApi.Model.Validators
{
    public class PastDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            if (date < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Date must be in the past");
        }
    }
}
