using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    class GenderValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string[] genders = { "Male", "Female", "Unknown" };
            foreach (string gender in genders)
            {
                if (gender.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
