using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    public class Customer
    {
        /// <summary>
        /// Unique number identifies the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mandatory value indicates the gender of the customer -
        /// possible values are: "Male", "Female", "Unknown"
        /// </summary>
        [Required]
        public Gender Gender { get; set; }

        /// <summary>
        /// Mandatory first name of the customer
        /// </summary>
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Mandatory last name of the customer
        /// </summary>
        [StringLength(75)]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Mandatory date of birth (no time)
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Mandatory name of the street
        /// </summary>
        [StringLength(75)]
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// Mandatory house number
        /// </summary>
        [StringLength(10)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Mandatory Zip code
        /// </summary>
        [StringLength(10)]
        [Required]
        public string ZipCode { get; set; }

        /// <summary>
        /// Mandatory name of the town
        /// </summary>
        [StringLength(75)]
        [Required]
        public string Town { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Unknown
    }
}
