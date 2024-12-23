﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
    public class User : IdentityUser
    {

        public string  FullName { get; set; }


        public bool IsAdmin { get; set; } = false;
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

        public List<Orders> UserOrders { get; set; } = new List<Orders>();

        //  public Comment Comments { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
