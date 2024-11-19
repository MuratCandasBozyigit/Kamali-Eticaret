using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class ShippingInfo
    {
        public int Id { get; set; }

        // Foreign key to Order
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string FullAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        // Regular expression for ZipCode (Country-specific)
        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip Code format.")] // US example
        public string ZipCode { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string PostalCode { get; set; }
    }
}
