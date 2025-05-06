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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace API.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static IServiceCollection AddConfigurationService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuditing().AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .AddAuditInterceptors());

            return services;
        }

        public static IServiceCollection AddAuditing(this IServiceCollection services,
            Action<AuditOptions> configureOptions = null)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<AuditService>();

            configureOptions?.Invoke(new AuditOptions(services));
            return services;
        }

        public static DbContextOptionsBuilder AddAuditInterceptors(this DbContextOptionsBuilder optionsBuilder)
        {
            var coreOptionsExtension = optionsBuilder.Options.GetExtension<CoreOptionsExtension>();
            var clonedCoreOptionsExtension = new CoreOptionsExtension()
                .WithApplicationServiceProvider(coreOptionsExtension.ApplicationServiceProvider);

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder)
                .AddOrUpdateExtension(clonedCoreOptionsExtension);

            optionsBuilder.AddInterceptors(
                coreOptionsExtension.ApplicationServiceProvider!.GetRequiredService<AuditService>());

            return optionsBuilder;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddSingleton<IMinioClientFactory, MinioClientFactory>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IContentService, ContentService>();

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