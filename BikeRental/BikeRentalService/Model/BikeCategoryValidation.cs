using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    class BikeCategoryValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string[] categories = { "Standard bike", "Mountainbike", "Trecking bike", "Racing bike" };
            foreach (string category in categories)
            {
                if (category.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
