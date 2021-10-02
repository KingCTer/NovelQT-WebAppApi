using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NovelQT.Application.AutoMapper;
using System;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class AutoMapperExtension
{
    internal static WebApplicationBuilder AddAutoMapperConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Services.AddAutoMapper(AutoMapperConfig.RegisterMappings());

        return builder;
    }
}
