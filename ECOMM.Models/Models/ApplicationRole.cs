using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ECOMM.Core.Models
{
    public class ApplicationRole : IdentityRole
    {

        public string RoleName { get; set; }
        public string Description { get; set; }


        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}