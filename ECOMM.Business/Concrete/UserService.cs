using ECOMM.Data.Shared.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECOMM.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace ECOMM.Business.Concrete
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
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
    }
}
