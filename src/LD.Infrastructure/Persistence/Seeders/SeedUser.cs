using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Infrastructure.Persistence.Seeders
{
    public static class SeedUser
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByNameAsync("admin");

            if (user == null)
            {
                var devUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@ld.com",
                    FullName = "Administrador Dev",
                    IsActive = true
                };

                await userManager.CreateAsync(devUser, "Admin123!");
            }
        }
    }
}
