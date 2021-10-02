using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NovelQT.Infra.CrossCutting.Identity.Context;
using NovelQT.Infra.CrossCutting.Identity.IdentityServer;
using NovelQT.Infra.CrossCutting.Identity.IdentityServer.Services;
using NovelQT.Infra.CrossCutting.Identity.Models;
using System;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class IdentityExtension
{
    internal static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<AppUser>()
            .AddProfileService<IdentityProfileService>()
            .AddDeveloperSigningCredential();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            //options.Password.RequiredLength = 8;
            //options.Password.RequireDigit = true;
            //options.Password.RequireUppercase = true;
            //options.User.RequireUniqueEmail = true;
        });

        return builder;
    }
}
