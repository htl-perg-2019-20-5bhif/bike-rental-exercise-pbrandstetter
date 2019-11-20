using BikeRentalService.Model;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalApi.Model.Validators
{
    public class GenderValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;
            string[] genders = { "Male", "Female", "Unknown" };
            foreach (string gender in genders)
            {
                if (gender.Equals(customer.Gender))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Gender must be \"Male\", \"Female\", \"Unknown\"");
        }
    }
}
