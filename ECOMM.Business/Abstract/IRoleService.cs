using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface IRoleService
    {
        List<ApplicationRole> GetAllRoles();
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<ApplicationRole> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> UpdateRoleAsync(ApplicationRole role);

    }
}
