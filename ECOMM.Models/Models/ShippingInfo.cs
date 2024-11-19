using System;
using System.ComponentModel.DataAnnotations;

namespace ECOMM.Core.Models
{
    public class ShippingInfo
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key to Orders
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        // Shipping Information
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip Code format.")]
        public string ZipCode { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string PostalCode { get; set; }

        // Shipping Address Fields
        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; } // Optional second line of address

        [Required]
        public string PostalCodeShipping { get; set; }

        // User information, assuming a one-to-many relationship between User and ShippingInfo
        public string UserId { get; set; } // This links to the User model (assumed to exist)

        // Navigation property to User
        public virtual User User { get; set; }
    }
}
