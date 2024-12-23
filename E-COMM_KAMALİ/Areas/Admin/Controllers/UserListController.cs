﻿using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class UserListController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserListController(IRoleService roleService, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager, IUserService userService)
        {
            _roleService = roleService;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                user.UserRoles = userRoles.Select(role => new ApplicationUserRole
                {
                    Role = roles.FirstOrDefault(r => r.Name == role),
                    UserId = user.Id
                }).ToList();
            }

            ViewBag.AllRoles = roles;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (user == null || role == null)
            {
                return BadRequest("Kullanıcı veya rol bulunamadı.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains(role.Name))
            {
                return BadRequest("Kullanıcı zaten bu role sahiptir.");
            }

            if (currentRoles.Any())
            {
                var oldRole = currentRoles.First();
                await _userManager.RemoveFromRoleAsync(user, oldRole);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return Ok("Rol başarıyla atandı.");
            }

            return BadRequest("Rol atama işlemi başarısız oldu.");
        }

        [HttpGet("GetAllRolesAsync")]
        public IActionResult GetAllRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return Json(roles);
        }

        #region GetRoleById
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById([FromQuery] string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                return BadRequest("Rol ID'si boş veya geçersiz.");

            var role = await _roleService.GetRoleByIdAsync(roleId);

            if (role == null)
                return NotFound("Rol bulunamadı.");

            return Json(role);
        }
        #endregion

        #region Update 
        [HttpPost("UpdateRoleAsync")]
        public async Task<IActionResult> UpdateRoleAsync([FromQuery] string roleId, [FromBody] ApplicationRole model)
        {
            if (roleId == null || model == null)
            {
                return BadRequest("Geçersiz veri.");
            }

            try
            {
                var existingRole = await _roleService.GetRoleByIdAsync(roleId);

                if (existingRole == null)
                {
                    return NotFound("Rol bulunamadı.");
                }

                existingRole.RoleName = model.RoleName;
                existingRole.Description = model.Description;
                existingRole.Name = model.RoleName;
                existingRole.NormalizedName = model.RoleName.ToUpper();

                var result = await _roleService.UpdateRoleAsync(existingRole);

                if (result.Succeeded)
                {
                    return Ok("Rol başarıyla güncellendi.");
                }

                return BadRequest("Rol güncelleme işlemi başarısız oldu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
        #endregion

        #region CreateRole 
        [HttpGet("CreateRole")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole
                {
                    RoleName = model.RoleName,
                    Description = model.Description,
                    Name = model.RoleName,
                    NormalizedName = model.RoleName.ToUpper()
                };

                var result = await _roleService.CreateRoleAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Rol başarıyla oluşturuldu.");
                }

                return BadRequest("Rol oluşturma işlemi başarısız oldu.");
            }

            return BadRequest("Geçersiz veri.");
        }
        #endregion

        #region DeleteRole 
        [HttpPost("DeleteAsyncRoles")]
        public async Task<IActionResult> DeleteAsyncRoles([FromBody] string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return BadRequest("Rol ID'si boş olamaz.");
            }

            var result = await _roleService.DeleteRoleAsync(roleId);

            if (result.Succeeded)
            {
                return Ok("Rol başarıyla silindi.");
            }

            return BadRequest("Rol silme işlemi başarısız oldu."); 
        }
        #endregion
    }
}
