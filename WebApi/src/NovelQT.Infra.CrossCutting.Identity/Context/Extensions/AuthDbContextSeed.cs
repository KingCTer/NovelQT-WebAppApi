using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NovelQT.Domain.Common.Constants;
using NovelQT.Infra.CrossCutting.Identity.Models;
using System;
using System.Linq;

namespace NovelQT.Infra.CrossCutting.Identity.Context.Extensions
{
    public class AuthDbContextSeed
    {
        public static bool EnsureSeedData(string connectionString)
        {
            if (connectionString == null) return false;

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AuthDbContext>(options => {
                options.UseSqlServer(connectionString, o => o.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
            });
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<AuthDbContext>();

            if (context == null) return false;
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var adminUser = userMgr.FindByNameAsync("admin").Result;
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Id = "admin",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = userMgr.CreateAsync(adminUser, "Admin@123$").Result;
                if (result.Succeeded)
                {
                    var user = userMgr.FindByNameAsync("admin").Result;
                    result = userMgr.AddToRoleAsync(user, IdentityConstant.Roles.AdminId).Result;
                }
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            else
            {
            }

            var userUser = userMgr.FindByNameAsync("user").Result;
            if (userUser == null)
            {
                userUser = new AppUser
                {
                    Id = "user",
                    UserName = "user",
                    Email = "user@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = userMgr.CreateAsync(userUser, "Admin@123$").Result;
                if (result.Succeeded)
                {
                    var user = userMgr.FindByNameAsync("user").Result;
                    result = userMgr.AddToRoleAsync(user, IdentityConstant.Roles.UserId).Result;
                }
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

            }
            else
            {
            }

            return true;
        }
    }
}
