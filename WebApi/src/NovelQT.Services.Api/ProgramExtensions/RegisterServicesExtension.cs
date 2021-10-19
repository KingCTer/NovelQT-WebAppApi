using Microsoft.AspNetCore.Builder;
using NovelQT.Infra.CrossCutting.IoC;
using NovelQT.Infra.Data.Context;
using System;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class RegisterServicesExtension
{
    internal static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        // Adding dependencies from another layers (isolated from Presentation)
        NativeInjectorBootStrapper.RegisterServices(builder.Services);

        return builder;
    }

    internal static WebApplicationBuilder RegisterContextFactory(this WebApplicationBuilder builder, ApplicationDbContextFactory dbContextFactory)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        // Adding dependencies from another layers (isolated from Presentation)
        NativeInjectorBootStrapper.RegisterContextFactory(builder.Services, dbContextFactory);

        return builder;
    }
}
