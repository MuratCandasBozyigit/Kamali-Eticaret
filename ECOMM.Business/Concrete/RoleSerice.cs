using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Concrete
{
    public class RoleSerice
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleSerice(RoleManager<ApplicationRole> roleManager)
        {
            this._roleManager = roleManager;
        }


        public List<ApplicationRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return await _roleManager.DeleteAsync(role);
        }
        public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id);

            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                existingRole.NormalizedName = role.NormalizedName;
                existingRole.Description = role.Description;

                return await _roleManager.UpdateAsync(existingRole);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Rol bulunamadı." });
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

    }
}
