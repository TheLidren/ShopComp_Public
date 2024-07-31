using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopComp.Models;
using ShopComp.ViewModels.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopComp.Controllers
{
    public class RolesController : Controller
    {

        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        User user;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> EditRole(string userId)
        {
            user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new()
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                if (roles.Count == 0)
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    return await EditRole(userId);
                }
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Account");
            }
            return NotFound();
        }

    }
}

