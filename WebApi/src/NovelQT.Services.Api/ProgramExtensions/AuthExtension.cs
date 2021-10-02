using Duende.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NovelQT.Services.Api.ProgramExtensions;
public static class AuthExtension
{
    public static WebApplicationBuilder AddAuthConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Services.AddLocalApiAuthentication();

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5000/signin-google
                options.ClientId = "378129271380-kgm3r5ifbaeio4ch1teep4kk5kfmi1pq.apps.googleusercontent.com";
                options.ClientSecret = "dbkwyozeyn4wf1hm3IBuyDJg";
            })
            .AddLocalApi("PrivateAccessToken", option =>
            {
                option.ExpectedScope = "PrivateApi";
            })
            ;

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("PrivateAccessToken", policy =>
            {
                policy.AddAuthenticationSchemes("PrivateAccessToken");
                policy.RequireAuthenticatedUser();
                //custom requirements
            });
        });

        return builder;
    }
}
