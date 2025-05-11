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
                    Name = "Admin",
                    Email = "admin@admin.com",
                    PhoneNumber = "0123456789",
                };
                
               await userManager.CreateAsync(admin, "admin123");
               await userManager.AddToRoleAsync(admin, nameof(Role.Admin));
               
               var teacher = new AppUser()
                {
                    UserName = "teacher",
                    Name = "teacher",
                    Email = "teacher@teacher.com",
                    PhoneNumber = "903182678",
                };
                
               await userManager.CreateAsync(teacher, "teacher123");
               await userManager.AddToRoleAsync(teacher, nameof(Role.Teacher));
             
               var student = new AppUser()
                {
                    UserName = "student",
                    Name = "student",
                    Email = "student@student.com",
                    PhoneNumber = "937072078",
                };
                
               await userManager.CreateAsync(student, "student123");
               await userManager.AddToRoleAsync(student, nameof(Role.Student));
            }
        }
    }
}
