using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECOMM.Business.Abstract;

namespace ECOMM.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService; // Injecting IOrderService to handle orders

        public UserService(UserManager<User> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        public async Task<User> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return user;
            }
            return null;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;

                await _userManager.UpdateAsync(existingUser);
                return existingUser;
            }
            return null;
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

    
    }
}
