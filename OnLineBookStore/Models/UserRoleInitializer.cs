using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace OnLineBookStore.Models
{
    public static class UserRoleInitializer
    {

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usermanager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] rolenames = { "Admin", "User" };
            IdentityResult roleresult;
            foreach (var role in rolenames) 
            {  
                var exists=await rolemanager.RoleExistsAsync(role);
                if (!exists)
                {
                    roleresult=await rolemanager.CreateAsync(role:new IdentityRole(role));
                }
            }
            var email = "admin@site.com";
            var password = "Qwerty123!";
            if(usermanager.FindByEmailAsync(email).Result==null)
            { 
                AppUser user= new()
                {
                    Email=email,
                    UserName=email,
                    FirstName="Admin",
                    LastName="Adminsoon",
                    Address="Adstreet 3",
                    City="Big City",
                    ZipCode="12345"
                };
                IdentityResult result = usermanager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    usermanager.AddToRoleAsync(user,role:"Admin").Wait();
                }
            }
           
        }
    }
}
