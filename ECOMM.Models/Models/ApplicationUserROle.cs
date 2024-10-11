using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
    public class ApplicationUserRole
    {

        public string UserId { get; set; }
        public User User { get; set; }


        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
    }
}