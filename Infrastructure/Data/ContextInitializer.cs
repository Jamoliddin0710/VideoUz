using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public  static class ContextInitializer
    {
        public static async Task Initialize(AppDbContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
          
            if ((await context.Database.GetPendingMigrationsAsync()).Count() > 0)
            {
               await context.Database.MigrateAsync();
            }

            if (!await context.AppRoles.AnyAsync())
            {
                string[] roles = Enum.GetNames<Role>();
                foreach (var role in roles)
                {
                   await roleManager.CreateAsync(new AppRole()
                   {
                       Name = role
                   });
                }
            }
            
            if (!await context.AppUsers.AnyAsync())
            {
                var admin = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    PhoneNumber = "0123456789",
                };
                
               await userManager.CreateAsync(admin, "admin123");
               await userManager.AddToRoleAsync(admin, Role.Admin.ToString());
               
               var moderator = new AppUser()
                {
                    UserName = "moderator",
                    Email = "moderator@moderator.com",
                    PhoneNumber = "0123456789",
                };
                
               await userManager.CreateAsync(moderator, "moderator123");
               await userManager.AddToRoleAsync(moderator, Role.Moderator.ToString());
             
               var user = new AppUser()
                {
                    UserName = "user",
                    Email = "user@user.com",
                    PhoneNumber = "0123456789",
                };
                
               await userManager.CreateAsync(user, "user123");
               await userManager.AddToRoleAsync(user, Role.User.ToString());
            }
        }
    }
}
