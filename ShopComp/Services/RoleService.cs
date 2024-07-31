using Microsoft.AspNetCore.Identity;
using ShopComp.Models;
using System.Threading.Tasks;

namespace ShopComp.Services
{
    public class RoleService
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "vlad.misevich.2003@mail.ru";
            string password = "_Aa123456";
            await roleManager.CreateAsync(new IdentityRole("user"));
            await roleManager.CreateAsync(new IdentityRole("admin"));
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new() { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
