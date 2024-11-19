using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{

    public class ShippingInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }

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
    }


}
