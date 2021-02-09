using leave_management.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management
{
    public static class SeedData
    {
        public static void Seed(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedData.SeedRole(roleManager);
            SeedData.SeedUser(userManager);
        }

        private static void SeedUser(UserManager<Employee> userManager)
        {
            if(userManager.FindByNameAsync("admin@localhost.com").Result == null)
            {
                var user = new Employee { UserName = "admin@localhost.com", Email = "admin@localhost.com" };
                var result = userManager.CreateAsync(user, "P@ssword123").Result;

                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }

            }
        }

        private static void SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Wait();
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Employee" }).Wait();
            }
        }
    }
}
