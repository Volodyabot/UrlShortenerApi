using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public static class InitialDataSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "User" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync("role"))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Admin",
                };

                var result = await userManager.CreateAsync(user, "Qwerty1/");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }

        public static async Task SeedDescriptionAsync(IServiceProvider serviceProvider)
        {
            var dataContext = serviceProvider.GetRequiredService<DataContext>();

            var exisistingDescription = await dataContext.Descriptions.FirstOrDefaultAsync(d => d.Id == 1);

            if (exisistingDescription == null)
            {
                var description = new Description
                {
                    UpdateDate = DateTime.UtcNow,
                    Value = "Please, update the description"
                };

                var result = dataContext.Descriptions.Add(description);

                await dataContext.SaveChangesAsync();
            }
        }
    }
}