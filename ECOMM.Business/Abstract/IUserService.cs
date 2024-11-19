using System.Collections.Generic;
using System.Threading.Tasks;
using ECOMM.Core.Models;

namespace ECOMM.Business.Abstract
{
    public interface IUserService
    {
        Task<User> DeleteUserAsync(string userId);
        Task<User> UpdateUserAsync(User user);
        Task<User> GetByIdAsync(string userId);
        Task<List<User>> GetAllUsersAsync();

   
    }
}
