using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalService.Model
{
    public class Bike
    {
        /// <summary>
        /// Unique number identifies a bike
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mandatory value indicates the brand of the bike
        /// </summary>
        [StringLength(25)]
        [Required]
        public string Brand { get; set; }

        /// <summary>
        /// Mandatory value indicates the date when the bike was purchased (no time)
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Required]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Optional notes or description for the bike
        /// </summary>
        [StringLength(1000)]
        public string Notes { get; set; }

        /// <summary>
        /// Indicates the date of the last service (no time)
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime LastService { get; set; }

        /// <summary>
        /// Mandatory rental price in Euro for first hour,
        /// Minimum value is 0.00
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal PriceFirstHour { get; set; }

        /// <summary>
        /// Mandatory rental price in Euro for each additional hour
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal PricePerAdditionalHour { get; set; }

        /// <summary>
        /// Possible bike categories are:
        /// "Standard bike", "Mountainbike", "Trecking bike", "Racing bike"
        /// </summary>
        [BikeCategoryValidation]
        public string BikeCategory { get; set; }
    }
}
