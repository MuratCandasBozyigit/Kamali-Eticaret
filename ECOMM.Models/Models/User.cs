using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ECOMM.Core.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? ProfilePictureUrl { get; set; }

        public bool IsAdmin { get; set; } = false;
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
