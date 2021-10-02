using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NovelQT.Infra.Data.Context;
using System;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class HealthCheckExtension
{
    internal static WebApplicationBuilder AddHealthCheckConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        if (builder.Environment.IsProduction() || builder.Environment.IsStaging())
        {
            builder.Services.AddHealthChecks()
                    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .AddDbContextCheck<ApplicationDbContext>();

            //builder.Services.AddHealthChecksUI(opt =>
            //{
            //    opt.SetEvaluationTimeInSeconds(15); // time in seconds between check
            //    opt.DisableDatabaseMigrations();
            //}).AddInMemoryStorage();
        }

        return builder;
    }

    internal static void UseHealthCheckConfiguration(IEndpointRouteBuilder endpoints, IWebHostEnvironment env)
    {
        if (env.IsProduction() || env.IsStaging())
        {
            endpoints.MapHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //endpoints.MapHealthChecksUI(setup =>
            //{
            //    setup.UIPath = "/hc-ui";
            //    setup.ApiPath = "/hc-json";
            //});
        }
    }
}
