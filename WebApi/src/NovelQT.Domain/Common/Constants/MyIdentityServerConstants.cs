namespace NovelQT.Domain.Common.Constants
{
    public static class MyIdentityServerConstants
    {
        //
        // Summary:
        //     Constants for local IdentityServer access token authentication.
        public static class PrivateApi
        {
            //
            // Summary:
            //     The authentication scheme when using the AddLocalApi helper.
            public const string AuthenticationScheme = "PrivateAccessToken";

            //
            // Summary:
            //     The API scope name when using the AddLocalApi helper.
            public const string ScopeName = "PrivateApi";

            //
            // Summary:
            //     The authorization policy name when using the AddLocalApi helper.
            public const string PolicyName = "PrivateAccessToken";
        }
    }
}
