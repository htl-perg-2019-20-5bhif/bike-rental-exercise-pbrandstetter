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
        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Optional notes or description for the bike
        /// </summary>
        [StringLength(1000)]
        public string Notes { get; set; }

        /// <summary>
        /// Indicates the date of the last service (no time)
        /// </summary>
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastService { get; set; }

        /// <summary>
        /// Mandatory rental price in Euro for first hour,
        /// Minimum value is 0.00
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public double PriceFirstHour { get; set; }

        /// <summary>
        /// Mandatory rental price in Euro for each additional hour
        /// Minimum value is 1.00
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public double PricePerAdditionalHour { get; set; }

        /// <summary>
        /// Possible bike categories are:
        /// "Standard bike", "Mountainbike", "Trecking bike", "Racing bike"
        /// </summary>
        [BikeCategoryValidation]
        public string BikeCategory { get; set; }
    }
}
