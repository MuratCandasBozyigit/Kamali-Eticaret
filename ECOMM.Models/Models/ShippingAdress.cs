using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ECOMM.Core.Models
{
    

    public class ShippingAddress
    {
        [Key] // Use [Key] for the primary key if you're using Entity Framework
        public int Id { get; set; }

        // If you want to ensure City is a required field, use [Required] attribute
        [Required]
        public string City { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; } // Optional second line of address

        [Required]
        public string PostalCode { get; set; }

        // If this address belongs to a User, create a foreign key relationship
        public string UserId { get; set; } // Assume that User has a one-to-many relation with ShippingAddresses

        // Navigation property to User (assuming User is another model in your application)
        public virtual User User { get; set; } // This makes it easier to navigate from ShippingAddress to User
    }


}
