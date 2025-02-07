using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;


namespace WebApplication7
{

    public static class AdminAccountInitializer
    {
        public static async Task EnsureAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string roleName = "Admin";
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                string username = configuration["AdminUser:Name"];
                string email = configuration["AdminUser:Email"];
                string password = configuration["AdminUser:Password"];

                if (await userManager.FindByNameAsync(username) == null)
                {
                    var user = new IdentityUser { UserName = username, Email = email };
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }
        }
    }
}
