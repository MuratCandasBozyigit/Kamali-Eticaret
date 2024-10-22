using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECOMM.Core.Models;
namespace ECOMM.Business.Abstract
{
    public interface IUserService 
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> DeleteUserAsync(string userId);
        Task<User> UpdateUserAsync(User user);
    }
}
