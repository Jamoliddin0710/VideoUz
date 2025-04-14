using System;
using Application.ServiceContract;
using Application.Services;
using Domain.Entities;
using Domain.RepositoryContracts;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static IServiceCollection AddConfigurationService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            return services;
        }
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                }).AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddUserManager<UserManager<AppUser>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromHours(20);
                });
            return services;
        }
    }
}
