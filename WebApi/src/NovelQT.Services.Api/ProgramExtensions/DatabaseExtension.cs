using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NovelQT.Infra.CrossCutting.Identity.Context;
using NovelQT.Infra.CrossCutting.Identity.Context.Extensions;
using NovelQT.Infra.Data.Context;
using Serilog;
using System;
using System.Linq;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class DatabaseExtension
{
    internal static WebApplicationBuilder AddDatabaseConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        // Add ApplicationDbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            if (!builder.Environment.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        // Add AuthDbContext
        builder.Services.AddDbContext<AuthDbContext>(options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName));
            if (!builder.Environment.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        // Add EventStoreSqlContext
        builder.Services.AddDbContext<EventStoreSqlContext>(options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly(typeof(EventStoreSqlContext).Assembly.FullName));
            if (!builder.Environment.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return builder;
    }

    internal static bool IsSeedData(ref string[] args)
    {
        bool IsSeed = args.Contains("/seed");
        if (IsSeed)
        {
            args = args.Except(new[] { "/seed" }).ToArray();
        }
        return IsSeed;
    }

    internal static WebApplication DatabaseExtensionSeedData(this WebApplication app, bool IsSeed)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        if (!IsSeed) return app;

        Log.Information("Seeding database...");
        var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
        var result = AuthDbContextSeed.EnsureSeedData(connectionString);
        if (result)
        {
            Log.Information("Done seeding database.");
        }
        else
        {
            Log.Information("Seeding database false.");
        }
        

        return app;
    }
}
