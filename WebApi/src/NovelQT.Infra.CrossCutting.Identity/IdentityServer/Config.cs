using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using NovelQT.Domain.Common.Constants;
using NovelQT.Infra.CrossCutting.Identity.IdentityServer.Models;
using System.Collections.Generic;

namespace NovelQT.Infra.CrossCutting.Identity.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                //new MyIdentityResources.Permissions(),
                //new MyIdentityResources.Roles(),
                
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName, "Public Api Resource"),
                //new ApiResource(MyIdentityServerConstants.PrivateApi.ScopeName, "Private Api Resource"),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, "Public Api Scope"),
                //new ApiScope(MyIdentityServerConstants.PrivateApi.ScopeName, "Private Api Scope"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "Swagger Client",
                    ClientSecrets = { new Secret("SwaggerSecret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Implicit,

                    AllowAccessTokensViaBrowser = true,

                    RequireConsent = false,

                    RedirectUris = { "https://localhost:5000/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "https://localhost:5000/swagger/oauth2-redirect.html" },

                    AllowedCorsOrigins = { "https://localhost:5000" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        MyIdentityServerConstants.PrivateApi.ScopeName,
                    }
                },
                new Client
                {
                    ClientId = "admin",
                    ClientName = "Admin Client",
                    ClientSecrets = { new Secret("AdminSecret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    //AllowOfflineAccess = true,

                    //RequireConsent = false,
                    //RequirePkce = true,

                    //RedirectUris = { "https://localhost:5001/signin-oidc" },
                    //FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    //PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                       // IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                    }
                },
                //new Client
                //{
                //    ClientId = "web",
                //    ClientSecrets = { new Secret("WebSecret".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RequireConsent = false,
                //    RequirePkce = true,
                //    AllowOfflineAccess = true,

                //    RedirectUris = { "https://localhost:5002/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                //    AllowedScopes = {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess,
                //        IdentityServerConstants.LocalApi.ScopeName,
                //    }
                //},
                new Client
                {
                    ClientId = "android",
                    RequireClientSecret = false,
                    //ClientSecrets = { new Secret("WebSecret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    AccessTokenLifetime = 7200,

                    RequireConsent = false,
                    RequirePkce = false,
                    AllowOfflineAccess = true,
                    RedirectUris = { "net.openid.appauthdemo:/oauth2redirect" },
                    FrontChannelLogoutUri = "net.openid.appauthdemo:/oauth2redirect",
                    PostLogoutRedirectUris = { "net.openid.appauthdemo:/oauth2redirect" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
                    }
                },

            };
    }
}